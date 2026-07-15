using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserControl.Core.Constants
{
    public static class ErrorCodes
    {

        public const string userNotFound = "453";
        public const string msgUserNotFound = "El usuario no existe";

        public const string userBlocked = "454";
        public const string smgUserBlocked = "El usuario está bloqueado";

        //Porconfirmar
        public const string confirmAccount = "455";
        public const string msgConfirmAccount = "El usuario debe confirmar su cuenta";

        //usuario ha sido bloqueado
        public const string userWasBlocked = "456";
        public const string msgUserWasBlocked = "El usuario ha sido bloqueado";

        //contraseña incorrecta
        public const string incorrectPassword = "457";
        public const string msgIncorrectPassword = "La contraseña es incorrecta";

        //sin permisos
        public const string noPermits = "458";
        public const string msgNoPermits = "No tiene permisos para ingresar al sistema";

        //cuenta ya existe
        public const string accountExists = "459";
        public const string msgAccountExists = "Ya existe una cuenta registrada con ese correo";

        //codigo incorrecto
        public const string incorrectCode = "460";
        public const string msgIncorrectCode = "El código es incorrecto";

        //accion no permitida
        public const string notAllowed = "461";
        public const string msgNotAllowed = "Acción no permitida para esta cuenta";

        //accion no permitida
        public const string tokenExpired = "462";
        public const string msgTokenExpired = "El token ha expirado";


        //error desconocido
        public const string defaultCode = "499";
        public const string msgDefaultCode = "Error desconocido";


        public static string GetErrorMessage(string code)
        {
            switch (code)
            {
                case userNotFound:
                    return msgUserNotFound;

                case userBlocked:
                    return smgUserBlocked;

                case confirmAccount:
                    return msgConfirmAccount;

                case userWasBlocked:
                    return msgUserWasBlocked;

                case incorrectPassword:
                    return msgIncorrectPassword;

                case noPermits:
                    return msgNoPermits;

                case accountExists:
                    return msgAccountExists;

                case incorrectCode:
                    return msgConfirmAccount;

                case notAllowed:
                    return msgNotAllowed;

                case tokenExpired:
                    return msgTokenExpired;

                default:
                    return msgDefaultCode;

            }
        }

    }
}
