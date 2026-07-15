using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using UserControl.Api.Responses;
using UserControl.Core.CustomEntities;
using UserControl.Core.DTOs;
using UserControl.Core.Entities;
using UserControl.Core.Interfaces;
using MEC = Microsoft.Extensions.Configuration;


using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Newtonsoft.Json;
using UserControl.Application.Interfaces.services;
using UserControl.Application.Responses;
using UserControl.Application.Interfaces;

namespace UserControl.Api.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;
        private readonly MEC.IConfiguration _configuration;

        public UserController(IUserService userService, IMapper mapper, IPasswordService passwordService, MEC.IConfiguration configuration)
        {
            _userService = userService;
            _mapper = mapper;
            _passwordService = passwordService;
            _configuration = configuration;
        }

        [Authorize]
        [HttpGet("Get/{UserId}")]
        public async Task<IActionResult> Get(int userId)
        {
            var user = await _userService.GetById(userId);
            var response = _mapper.Map<ApiResponses<UserDto>>(user);
           // var response = new ApiResponses<UserDto>(userDto);
            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> Post(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            user.Password = _passwordService.Hash(user.Password);
            var result = await _userService.RegisterUser(user);

            if (user != null)
            {
                //TODO: se debe mejorar este proceso de manera que se pueda guardar en cascada
                user.ProgramXUsers = new List<ProgramXUser> { new ProgramXUser(result.Id, 1, 5, "General") }; //  new ProgramXUser ( UserId = user.Id, ProgramId = 1 });

                var token = GenerateToken(user);
                userDto = _mapper.Map<UserDto>(user);
                userDto.Token = token;
                var response = new ApiResponses<UserDto>(userDto);
                return Ok(response);

            }
            return NotFound();
        }

        private string GenerateToken(User security)
        {
            //header

            var _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
            var signingCredentials = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            //claims
            //var claims = new[]
            //{
            //   new Claim("Id", security.Id.ToString()),
            //   new Claim("User", security.UserEmail),
            //   new Claim(ClaimTypes.Name, security.Name),
            //   new Claim(ClaimTypes.Role, security.Role.ToString()),
            //};

            // List<string, string> roles = new List<string, string>();

            // IList<string, string> list = new List<string, string>();


            //SE COMENTA PORQUE NO SE USA EL PROGRAMXUSERS
           /// string[,] hola = new string[2, security.ProgramXUsers.Count];



            int cont = 0;

            //security.ProgramXUsers.ForEach(x =>
            // {
            //     hola[0, cont] = x.ProgramId.ToString(),
            //     hola[1, cont] = x.RoleId.ToString()
            // }
            // );

            //security.ProgramXUsers.ForEach(x =>
            //{
            //    x.ProgramId = 1;
            //    x.RoleId = 2;
            //}
            //);


            //PROBAR ESTO
            //security.ProgramXUsers.ForEach(x =>
            // {
            //     hola[0, cont] = x.ProgramId.ToString();
            //     hola[1, cont] = x.RoleId.ToString();
            //     cont++;
            // }
            // );

            var lstRoles = new List<roles>();


            //ORIGINAL- se quita porque ProgramXUsers.ForEach no existia
            //security.ProgramXUsers.ForEach(x =>
            //{
            //    lstRoles.Add(
            //       new roles()
            //       {
            //           ProgramId = x.ProgramId,
            //           RoleId = x.RoleId
            //       }
            //  );
            //});

            //original
            //foreach ( var x in security.ProgramXUsers)
            //{
            //    lstRoles.Add(
            //       new roles()
            //       {
            //           ProgramId = x.ProgramId,
            //           RoleId = x.RoleId
            //       }
            //  );
            //};

            ////SE COMENTA PORQUE NO SE OCUPA EL PROGRAMA
            foreach (var x in security.ProgramXUsers)
            {
                lstRoles.Add(
                   new roles()
                   {

                       RoleName = x.RoleName
                   }
              );
            };


            String RolesJson = JsonConvert.SerializeObject(lstRoles);

            var claims = new[]
            {
               new Claim("Id", security.Id.ToString()),
               new Claim("User", security.Email),
               new Claim(ClaimTypes.Name, security.Names),
               new Claim(ClaimTypes.Role, RolesJson, JsonClaimValueTypes.JsonArray)
            };

            //Paylod

            var payload = new JwtPayload
            (
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claims,
                DateTime.Now,
                DateTime.UtcNow.AddDays(10)
            );

            //  DateTime.UtcNow.AddMinutes(10)

            //sign
            var token = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Edit(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
           // user.Password = _passwordService.Hash(user.Password);
           bool result = await _userService.EditUser(user);

           var response = new ApiResponses<bool>(result);
           return Ok(response);
        }


        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Authentication(UserLogin login)
        {
            //if is user valido
            var result = await _userService.GetLoginByCredentials(login);

            //   var validation = await IsValidUser(login);
            if (result != null)
            {
                var response = new ApiResponses<ResponseLogin>(result);
                return Ok(response);
            }

            return NotFound();
        }


        [Route("recoverpassword")]
        [HttpPost]
        public async Task<IActionResult> RecoverPassword(UserRecoverPassword recoverPassword)
        {
            //int result = 7;
            //if is user valido
            var result = await _userService.RecoverPassword(recoverPassword.Email);

            //   var validation = await IsValidUser(login);
            //if (result != null)
            //{
            //    var response = new ApiResponses<ResponseLogin>(result);
            //    return Ok(response);
            //}

            var response = new ApiResponses<bool>(result);

            return Ok(response);
        }

        //[Authorize]
        [Route("newpassword")]
        [HttpPost]
        public async Task<IActionResult> NewPassword(UserNewPassword userNewPassword)
        {
            //if is user valido
            var result = await _userService.NewPassword(userNewPassword);

            ////   var validation = await IsValidUser(login);
            //if (result != null)
            //{
            //    var response = new ApiResponses<ResponseLogin>(result);
            //    return Ok(response);
            //}
            var response = new ApiResponses<bool>(result);

            return Ok(response);
        }

        [Authorize]
        [Route("changepassword")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(UserChangePassword userChangePassword)
        {
            //if is user valido
            //var result = await _userService.GetLoginByCredentials(login);

            var result = await _userService.ChangePassword(userChangePassword);

            ////   var validation = await IsValidUser(login);
            //if (result != null)
            //{
            //    var response = new ApiResponses<ResponseLogin>(result);
            //    return Ok(response);
            //}

            var response = new ApiResponses<bool>(result);

            return Ok(response);
        }

        [Route("enableaccount")]
        [HttpPost]
        public async Task<IActionResult> EnableAccount(UserEnableAccount userEnableAccount)
        {
            //if is user valido
            var result = await _userService.EnableAccount(userEnableAccount);

            ////   var validation = await IsValidUser(login);
            //if (result != null)
            //{
            //    var response = new ApiResponses<ResponseLogin>(result);
            //    return Ok(response);
            //}

            var response = new ApiResponses<bool>(result);

            return Ok(response);
        }


        //[Route("ReenviarCodigo")]
        //[HttpPost]
        //public async Task<IActionResult> ReenviarCodigo(string email)
        //{
        //    //if is user valido
        //    var result = await _userService.EnableAccount(userEnableAccount);

        //    ////   var validation = await IsValidUser(login);

        //    var response = new ApiResponses<bool>(result);

        //    return Ok(response);
        //}


        [Route("confirmAccount")]
        [HttpPost]
        public async Task<IActionResult> ConfirmAccount(UserEnableAccount userEnableAccount)
        {
            //if is user valido
            var result = await _userService.EnableAccount(userEnableAccount);

            ////   var validation = await IsValidUser(login);
           
            var response = new ApiResponses<bool>(result);

            return Ok(response);
        }


        // [Route("requesttoken")]
        //[HttpGet("{email}")]
        //public async Task<IActionResult> RequestToken(string email)
        //{
        //    //if is user valido
        //    var result = await _userService.RequestToken(email);

        //    return Ok(result);
        //}

        [HttpGet("RequestToken/{email}")]
        public async Task<IActionResult> RequestToken(string email)
        {
            //if is user valido
            var result = await _userService.RequestToken(email);

            var response = new ApiResponses<bool>(result);

            return Ok(response);
        }



        //private async Task<(bool, User)> IsValidUser(UserLogin login)
        //{
        //    var user = await _userService.GetLoginByCredentials(login);
        //    var isValid = _passwordService.Check(user.Password, login.Password);

        //    return (isValid, user);
        //}


    }
}
