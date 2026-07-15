using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using UserControl.Application.Interfaces;
using UserControl.Application.Interfaces.services;
using UserControl.Application.Responses;
using UserControl.Core.Constants;
using UserControl.Core.CustomEntities;
using UserControl.Core.Entities;
using UserControl.Core.Enumerations;
using UserControl.Core.Exeptions;
using UserControl.Core.Interfaces;


namespace UserControl.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordService _passwordService;
        private readonly ISendEmailService _sendEmailService;

        public UserService(IUnitOfWork unitOfWork, IPasswordService passwordService, ISendEmailService sendEmailService)
        {
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
            _sendEmailService = sendEmailService;
        }


        public async Task<ApiResponses<User>> GetById(int id)
        {
            ResponseCode responseCode = new ResponseCode();

            var user = await _unitOfWork.UserRepository.GetById(id);

            if (user == null)
            {
                throw new BusinessExceptions(ErrorCodes.userNotFound);
            }

            if (user.State == UserState.Bloqueado)
            {
                throw new BusinessExceptions(ErrorCodes.userBlocked);
            }

            if (user.State == UserState.Porconfirmar)
            {
                //throw new BusinessExceptions(ErrorCodes.confirmAccount);
                responseCode.Code = ErrorCodes.confirmAccount;
                responseCode.Message = "Debes confirmar la cuenta";
            }

            var response = new ApiResponses<User>(user, responseCode);
           

            return response;
        }

        public async Task<ResponseLogin> GetLoginByCredentials(UserLogin userLogin)
        {
            int countAttempts;
            string numGenerate;

            ResponseCode responseCode = new ResponseCode();


            // MsnUserStatus
            //  return await _unitOfWork.UserRepository.GetLoginByCredentials(userLogin);

            var user = await _unitOfWork.UserRepository.GetLoginByCredentials(userLogin);

            //validate que exista
            if (user == null)
            {
                throw new BusinessExceptions(ErrorCodes.userNotFound);

                // throw new Exception(, ("El usuario no existe");
            }

            if (user.State == UserState.Bloqueado)
            {
                throw new BusinessExceptions(ErrorCodes.userBlocked);
            }

            if (user.State == UserState.Porconfirmar)
            {
                //throw new BusinessExceptions(ErrorCodes.confirmAccount);
                responseCode.Code = ErrorCodes.confirmAccount;
                responseCode.Message = "Debes confirmar la cuenta";

            }

            // if(user)
            var validation = IsValidUser(userLogin, user);

            //si la contrasena es incorrecta
            if (!validation)
            {
                countAttempts = user.Attempts + 1;

                if (countAttempts >= 5)
                {
                    user.State = UserState.Bloqueado;
                    user.Attempts = countAttempts;

                    numGenerate = GenerateRandom();
                    user.Token = numGenerate;

                    _unitOfWork.UserRepository.Update(user);
                    await _unitOfWork.SaveChangesAsyn();

                    SendEmail(user.Email, MsnUserStatus.blockCode, numGenerate);

                    throw new BusinessExceptions(ErrorCodes.userWasBlocked);

                }
                else
                {
                    user.Attempts = countAttempts;
                    _unitOfWork.UserRepository.Update(user);
                    await _unitOfWork.SaveChangesAsyn();

                    throw new BusinessExceptions(ErrorCodes.incorrectPassword);

                }
            }

            //var progxUser = await _unitOfWork.ProgramXUserRepository.GetRoleByUserProg(user.Id, (int)userLogin.program);

            var progxUser = _unitOfWork.ProgramXUserRepository.GetRoleByUserProg(user.Id);

            var accesToSistem = progxUser.Where(s => s.ProgramId == (int)userLogin.program).FirstOrDefault();

            // await _unitOfWork.SaveChangesAsyn();

            //EL USUARIO NO ESTA REGISTRADO EN ESTE PROGRAMA
            if (accesToSistem == null)
            {
                //throw new BusinessExceptions(ErrorCodes.noPermits);

                //agregar el programa al usuario

                var programUser = new ProgramXUser()
                {
                    UserId = user.Id,
                    ProgramId = (int)userLogin.program,
                    RoleId = (int)userLogin.rol == 0 ? (int)RoleType.Consumer : (int)userLogin.rol

                };

                await _unitOfWork.ProgramXUserRepository.Add(programUser);
                await _unitOfWork.SaveChangesAsyn();


                progxUser = _unitOfWork.ProgramXUserRepository.GetRoleByUserProg(user.Id);
            }

            // var lstRoles = 

            var responseLogin = new ResponseLogin
            {
                Id = user.Id,
                UserEmail = user.Email,
                Name = string.Format("{0} {1}", user.Names, user.SurNames),
                ProgramXUsers = progxUser, // Role = (RoleType)progxUser.RoleId
                ResponseCode = responseCode
            };

            return responseLogin;
        }


        public async Task<ResponseLogin> RegisterUser(User user)
        {
            string numGenerate;
            //TODO: se debe mejorar este proceso de manera que se pueda guardar en cascada

            //Reglas de negocio
            //1 validar si ya existe
            var userExist = await _unitOfWork.UserRepository.UserExists(user.Email);

            //validate que exista
            if (userExist)
            {
                throw new BusinessExceptions(ErrorCodes.accountExists);
            }

            user.State = UserState.Porconfirmar;
            //generar numero aleatorio
            numGenerate = GenerateRandom();
            user.Token = numGenerate;

            user.TokenExpiration = DateTime.UtcNow.AddMinutes(60);

            await _unitOfWork.UserRepository.Add(user);
            await _unitOfWork.SaveChangesAsyn();

            //TODO: el rol se debe traer del sistema talvez
            var progXUser = new ProgramXUser
            {
                UserId = user.Id,
                
                //ProgramId = (int)user.program,
                //antes se tomaba del programa al que venia ahora se deja un solo programa ahora no importa porque queda abierto a loguearse en cualquier programa
                ProgramId = (int)ProgramName.Eventos,

                // RoleId = (int)RoleType.Consumer
                //Todo: se debe hacer un mecanismo para definir el tipo
                //de usuario que se insertará, sobre todo cuando se implmente
                //usuarios consumer y operadores
                // RoleId = (int)RoleType.ProfileAdmin

                //Se coloca este rol para todos los usuarios porque se va a manejar roles en cada sistema conectado a este sistema de autenticacion
                RoleId = (int)RoleType.General
            };

            await _unitOfWork.ProgramXUserRepository.Add(progXUser);
            await _unitOfWork.SaveChangesAsyn();

            //envio de correo  se debe enviar por parametro tambien el correo
            //  SendEmail(user.Email, MsnUserStatus.confirmCode, numGenerate);


            //SE COMENTA TEMPORALMENTE
            //// SendEmail(user.Email, MsnUserStatus.confirmCode, numGenerate);


            //devuelve los datos del usuario para crear el token
            var responseLogin = new ResponseLogin
            {
                Id = user.Id,
                UserEmail = user.Email,
                Name = string.Format("{0} {1}", user.Names, user.SurNames),
                /// ProgramXUsers = new List<ProgramXUser>().Add(progXUser ) // Role = (RoleType)progxUser.RoleId
                ProgramXUsers = new List<ProgramXUser>() { new ProgramXUser(progXUser.UserId, progXUser.ProgramId, progXUser.RoleId, progXUser.RoleName) }
            };

            return responseLogin;

        }


        public async Task<bool> EditUser(User user)
        {
            // var userExist = await _unitOfWork.UserRepository.GetByEmail(user.Email);

            var userExist = await _unitOfWork.UserRepository.GetById(user.Id);

            //validate que exista
            if (userExist == null) //si no existe
            {
                throw new BusinessExceptions(ErrorCodes.accountExists);
            }

            if (userExist.State == UserState.Bloqueado)
            {
                throw new BusinessExceptions(ErrorCodes.userBlocked);
            }

            if (userExist.State != UserState.Activo)
            {
                throw new BusinessExceptions(ErrorCodes.notAllowed);
            }

            userExist.Names = user.Names;
            userExist.SurNames = user.SurNames;
            userExist.BirthDate = user.BirthDate;
            userExist.Phone = user.Phone;

            _unitOfWork.UserRepository.Update(userExist);
            await _unitOfWork.SaveChangesAsyn();

            //envio de correo  se debe enviar por parametro tambien el correo
            SendEmail(user.Email, MsnUserStatus.editCode);

            return true;
        }


        public async Task<bool> RecoverPassword(string email)
        {
            string numGenerate;

            var user = await _unitOfWork.UserRepository.GetByEmail(email);

            //validate que exista
            if (user == null)
            {
                throw new BusinessExceptions(ErrorCodes.userNotFound);
            }

            //if (user.State == UserState.Bloqueado)
            //{
            //    throw new BusinessExceptions(ErrorCodes.userBlocked);
            //}


            if (user.State != UserState.Activo && user.State != UserState.Bloqueado)
            {
                throw new BusinessExceptions(ErrorCodes.notAllowed);
            }

            //the person forgiven the password and he need created a new password
            //user.State = UserState.RecobrarContrasena;


            numGenerate = GenerateRandom();
            user.Token = numGenerate;
            user.TokenExpiration = DateTime.UtcNow.AddMinutes(15);

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsyn();

            //SE COMENTA TEMPORALMENTE
            //// SendEmail(user.Email, MsnUserStatus.recoverCode, numGenerate);

            return true;
        }

        public async Task<bool> NewPassword(UserNewPassword userNewPassword)
        {
            var user = await _unitOfWork.UserRepository.GetByEmail(userNewPassword.Email);

            //validate que exista
            if (user == null)
            {
                throw new BusinessExceptions(ErrorCodes.userNotFound);
            }

            //if (user.State != UserState.RecobrarContrasena)
            //{
            //    throw new BusinessExceptions(ErrorCodes.notAllowed);
            //}

            if (user.State != UserState.Activo && user.State != UserState.Bloqueado)
            {
                throw new BusinessExceptions(ErrorCodes.notAllowed);
            }

            if (user.TokenExpiration < DateTime.UtcNow)
            {
                throw new BusinessExceptions(ErrorCodes.tokenExpired);
            }

            //validar codigo
            //TODO
            //validar si se debe restringir la cantidad de codigos a ingresar
            if (userNewPassword.Code != user.Token)
            {
                throw new BusinessExceptions(ErrorCodes.incorrectCode);
            }

            //TODO
            //validar que la contrasena sea distinta a la anterior
            //PENDIENTE

            //TODO
            //considerar mas adelante guardar un historial de password

            //TODO
            //en la capa de api ya se debe validar el formato del password

            //elimina el token
            user.Token = "";
            user.Attempts = 0;
            user.State = UserState.Activo;

            //ESTO SE COMENTA PORQUE SE LE VA A DAR UNA CONTRASENA TEMPORAL
            //hastear pass
            //user.Password = _passwordService.Hash(userNewPassword.NewPassword);

            string newPassword = GenerateRandomPassword(8);
            user.Password = _passwordService.Hash(newPassword); ;

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsyn();

            //TODO
            //considerar si se envia notificacion de correo indicando el cambio de password, creo q si
            SendEmail(user.Email, MsnUserStatus.passCode, newPassword);

            return true;
        }

        public async Task<bool> ChangePassword(UserChangePassword userChangePassword)
        {
            int countAttempts;
            string numGenerate;

            //var user = await _unitOfWork.UserRepository.GetByEmail(userChangePassword.Email);

            var user = await _unitOfWork.UserRepository.GetById(userChangePassword.Id);

            //validate que exista
            if (user == null)
            {
                throw new BusinessExceptions(ErrorCodes.userNotFound);
            }

            if (user.State != UserState.Activo)
            {
                throw new BusinessExceptions(ErrorCodes.notAllowed);
            }

            //validar codigo
            //TODO
            //validar si se debe restringir la cantidad de codigos a ingresar
            //if (userNewPassword.Code != user.Token)
            //{
            //    throw new BusinessExceptions("Código incorrecto");
            //}

            var isValid = _passwordService.Check(user.Password, userChangePassword.PasswordActual);

            if (!isValid)
            {
                countAttempts = user.Attempts + 1;
                if (countAttempts >= 5)
                {
                    user.State = UserState.Bloqueado;
                    user.Attempts = countAttempts;

                    numGenerate = GenerateRandom();
                    user.Token = numGenerate;

                    _unitOfWork.UserRepository.Update(user);
                    await _unitOfWork.SaveChangesAsyn();

                    SendEmail(user.Email, MsnUserStatus.blockCode, numGenerate);

                    throw new BusinessExceptions(ErrorCodes.userWasBlocked);
                }
                else
                {
                    user.Attempts = countAttempts;
                    _unitOfWork.UserRepository.Update(user);
                    await _unitOfWork.SaveChangesAsyn();

                    throw new BusinessExceptions(ErrorCodes.incorrectPassword);
                }
            }

            user.Password = _passwordService.Hash(userChangePassword.PasswordNueva);
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsyn();

            //TODO
            //mandar email para avisar que hubo un cambio de contrasena
            SendEmail(user.Email, MsnUserStatus.changeCode);

            return true;
        }


        public async Task<bool> EnableAccount(UserEnableAccount userEnableAccount)
        {
            var user = await _unitOfWork.UserRepository.GetByEmail(userEnableAccount.Email);

            //validate que exista
            if (user == null)
            {
                throw new BusinessExceptions(ErrorCodes.userNotFound);
            }

            //if (user.State != UserState.Bloqueado && user.State != UserState.Porconfirmar)
            //{
            //    throw new BusinessExceptions(ErrorCodes.notAllowed);
            //}


            if (user.State != UserState.Porconfirmar)
            {
                throw new BusinessExceptions(ErrorCodes.notAllowed);
            }


            if (user.TokenExpiration < DateTime.UtcNow)
            {
                throw new BusinessExceptions(ErrorCodes.tokenExpired);
            }

            //TODO
            //se debe agregar funcionalidad para limitar el numero de intentos con el codigo incorrecto
            if (user.Token != userEnableAccount.Code)
            {
                throw new BusinessExceptions(ErrorCodes.incorrectCode);
            }

            user.Token = "";
            user.State = UserState.Activo;
            //hastear pass
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsyn();

            //TODO
            //mandar email de confirmación de cuenta activada
            SendEmail(user.Email, MsnUserStatus.activeCode);

            return true;
        }


        //public async Task<bool> ReenviarCodigo(string email)
        //{
        //    var user = await _unitOfWork.UserRepository.GetByEmail(email);

        //    //validate que exista
        //    if (user == null)
        //    {
        //        throw new BusinessExceptions(ErrorCodes.userNotFound);
        //    }

        //    //if (user.State != UserState.Bloqueado && user.State != UserState.Porconfirmar)
        //    //{
        //    //    throw new BusinessExceptions(ErrorCodes.notAllowed);
        //    //}


        //    if (user.State != UserState.Porconfirmar)
        //    {
        //        throw new BusinessExceptions(ErrorCodes.notAllowed);
        //    }


        //    if (user.TokenExpiration < DateTime.UtcNow)
        //    {
        //        throw new BusinessExceptions(ErrorCodes.tokenExpired);
        //    }

        //    //TODO
        //    //se debe agregar funcionalidad para limitar el numero de intentos con el codigo incorrecto
        //    if (user.Token != userEnableAccount.Code)
        //    {
        //        throw new BusinessExceptions(ErrorCodes.incorrectCode);
        //    }

        //    user.Token = "";
        //    user.State = UserState.Activo;
        //    //hastear pass
        //    _unitOfWork.UserRepository.Update(user);
        //    await _unitOfWork.SaveChangesAsyn();

        //    //TODO
        //    //mandar email de confirmación de cuenta activada
        //    SendEmail(user.Email, MsnUserStatus.activeCode);

        //    return true;
        //}


        public async Task<bool> ConfirmAccount(UserEnableAccount userEnableAccount)
        {

            var user = await _unitOfWork.UserRepository.GetByEmail(userEnableAccount.Email);

            //validate que exista
            if (user == null)
            {
                throw new BusinessExceptions(ErrorCodes.userNotFound);
            }

            if (user.State != UserState.Porconfirmar)
            {
                throw new BusinessExceptions(ErrorCodes.notAllowed);
            }

            //TODO
            //se debe agregar funcionalidad para limitar el numero de intentos con el codigo incorrecto
            if (user.Token != userEnableAccount.Code)
            {
                throw new BusinessExceptions(ErrorCodes.incorrectCode);
            }

            user.Token = "";
            user.State = UserState.Activo;
            //hastear pass
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsyn();

            //TODO
            //mandar email de confirmación de cuenta activada
            SendEmail(user.Email, MsnUserStatus.activeCode);

            return true;

        }

        public async Task<bool> RequestToken(string email)
        {

            var user = await _unitOfWork.UserRepository.GetByEmail(email);

            if (user == null)
            {
                throw new BusinessExceptions(ErrorCodes.userNotFound);

                // throw new Exception(, ("El usuario no existe");
            }


            //if (user.State != UserState.Bloqueado && user.State != UserState.Porconfirmar)

            if (user.State != UserState.Bloqueado && user.State != UserState.Activo && user.State != UserState.Porconfirmar)
            {
                throw new BusinessExceptions(ErrorCodes.notAllowed);
            }


            if (user.State == UserState.Bloqueado)
            {
                string numGenerate = GenerateRandom();
                user.Token = numGenerate;
                user.TokenExpiration = DateTime.UtcNow.AddMinutes(15);

                _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.SaveChangesAsyn();

                //  SendEmail(user.Email, MsnUserStatus.blockCode, numGenerate);

                SendEmail(email, MsnUserStatus.blockCode, numGenerate);

            }


            //por confirmar
            if (user.State == UserState.Porconfirmar || user.State == UserState.Activo)
            {

                string numGenerate = GenerateRandom();
                user.Token = numGenerate;
                user.TokenExpiration = DateTime.UtcNow.AddMinutes(15);

                _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.SaveChangesAsyn();

                // SendEmail(user.Email, MsnUserStatus.confirmCode, numGenerate);

                SendEmail(email, MsnUserStatus.reenviarCode, numGenerate);

            }


            return true;
        }


        private void SendEmail(string email, int state, string numRamdon = null)
        {
            try
            {
                //   var message = EmailMessagesUserStatus.GetEmailMessage(state);
                // Correo de texto
                //  _sendEmailService.SendEmailAsync("jcorderortiz@gmail.com","Prueba","Código de verificación Buscadme: " + numRamdon);

                string cuerpo;
                var message = MsnUserStatus.GetEmailMessage(state);

                if (state == 8 || state == 9)
                {
                    cuerpo = string.Format(message.Item2, numRamdon);
                }
                else if (numRamdon != null)
                {
                    cuerpo = string.Format(message.Item2, numRamdon, "http://cuentas.buscadme.org/Login/ConfirmAccount?email=" + email + "&code=" + numRamdon);
                }
               
                else
                {
                    cuerpo = message.Item2;
                }


                _sendEmailService.SendEmailAsync(email, message.Item1, cuerpo);


                ////*********************************************
                ////string emailOrigen = "jcorderortiz@gmail.com";
                ////string emailDestino = "jcorderortiz@gmail.com";
                ////string pass = "Novo7531";
                //string emailOrigen = "infobuscadme@gmail.com";
                //string emailDestino = "infobuscadme@gmail.com";
                //string pass = "Sion7531";
                //MailMessage oMailMessage = new MailMessage(emailOrigen, emailDestino, "Titulo", "cuerpo del mensaje");
                //oMailMessage.IsBodyHtml = true;
                //SmtpClient oSmtpClient = new SmtpClient("smtp.gmail.com");
                //oSmtpClient.EnableSsl = true;
                //oSmtpClient.UseDefaultCredentials = false;
                //oSmtpClient.Port = 587;
                //oSmtpClient.Credentials = new System.Net.NetworkCredential(emailOrigen, pass);
                //oSmtpClient.Send(oMailMessage);
                //oSmtpClient.Dispose();
                ////*********************************************

            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private string GenerateRandom()
        {
            string numGenerate;

            Random rdn = new Random();
            int num1 = rdn.Next(100, 999);
            int num2 = rdn.Next(100, 999);
            int num3 = rdn.Next(100, 999);

            numGenerate = string.Format("{0}-{1}-{2}", num1.ToString(), num2.ToString(), num3.ToString());

            return numGenerate;
        }

        private bool IsValidUser(UserLogin login, User user)
        {
            // var user = await _securityService.GetLoginByCredentials(login);
            //                                    servidor          logueo
            var isValid = _passwordService.Check(user.Password, login.Password);

            return isValid;
        }

        private string GenerateRandomPassword(int length)
        {

            const string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_+~`|}{[]\\:;?><,./-=\"'";

            var random = new Random();
            var password = new string(Enumerable.Repeat(characters, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return password;
        }


    }
}
