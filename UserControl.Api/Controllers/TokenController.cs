using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UserControl.Core.Entities;
using UserControl.Core.Enumerations;
using UserControl.Core.Interfaces;
using UserControl.Infrastructure.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserControl.Core.CustomEntities;
using System.Collections.Generic;
using Newtonsoft.Json;
using UserControl.Application.Interfaces.services;
using UserControl.Application.Responses;
using UserControl.Application.Interfaces;
using UserControl.Api.Responses;
using UserControl.Core.DTOs;

namespace UserControl.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly ISecurityService _securityService;
        private readonly IPasswordService _passwordService;
        private readonly IUserService _userService;

        public TokenController(IConfiguration configuration, ISecurityService securityService, IPasswordService passwordService, IUserService userService)
        {
            _configuration = configuration;
            _securityService = securityService;
            _passwordService = passwordService;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Authentication(UserLogin login)
        {
            //if is user valido
            var validation = await IsValidUser(login);
            if (validation.Item1)
            {
                var token = GenerateToken(validation.Item2);

                var response = new ApiResponses<string>(token, validation.Item2.ResponseCode);

                return Ok(response);
            }

            return NotFound();
        }

        //private async Task<(bool, Security)> IsValidUser(UserLogin login)
        private async Task<(bool, ResponseLogin)> IsValidUser(UserLogin login)
        {
            // var user = await _securityService.GetLoginByCredentials(login);

            var user = await _userService.GetLoginByCredentials(login);
           ///// var isValid = _passwordService.Check(user.Password, login.Password);

            var isValid = user != null ? true : false;

            return (isValid, user);
        }


        /// <summary>
        /// ESTE ES EL ORIGINAL, RESPALDO
        /// </summary>
        /// <param name="security"></param>
        /// <returns></returns>
        //private string GenerateToken(ResponseLogin security)
        //{
        //    //header
        //    var _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
        //    var signingCredentials = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        //    var header = new JwtHeader(signingCredentials);

        //    //claims
        //    //var claims = new[]
        //    //{
        //    //   new Claim("Id", security.Id.ToString()),
        //    //   new Claim("User", security.UserEmail),
        //    //   new Claim(ClaimTypes.Name, security.Name),
        //    //   new Claim(ClaimTypes.Role, security.Role.ToString()),
        //    //};

        //    // List<string, string> roles = new List<string, string>();

        //    // IList<string, string> list = new List<string, string>();

        //    string[,] hola = new string[2, security.ProgramXUsers.Count];

        //    int cont = 0;

        //    //security.ProgramXUsers.ForEach(x =>
        //    // {
        //    //     hola[0, cont] = x.ProgramId.ToString(),
        //    //     hola[1, cont] = x.RoleId.ToString()
        //    // }
        //    // );

        //    //security.ProgramXUsers.ForEach(x =>
        //    //{
        //    //    x.ProgramId = 1;
        //    //    x.RoleId = 2;
        //    //}
        //    //);


        //    //PROBAR ESTO
        //    //security.ProgramXUsers.ForEach(x =>
        //    // {
        //    //     hola[0, cont] = x.ProgramId.ToString();
        //    //     hola[1, cont] = x.RoleId.ToString();
        //    //     cont++;
        //    // }
        //    // );

        //    var lstRoles = new List<roles>();

        //    security.ProgramXUsers.ForEach(x =>
        //    {
        //        lstRoles.Add(
        //           new roles()
        //           {
        //               ProgramId = x.ProgramId,
        //               RoleId = x.RoleId
        //           }
        //      );
        //    });


        //    String RolesJson = JsonConvert.SerializeObject(lstRoles);

        //    var claims = new[]
        //    {
        //       new Claim("Id", security.Id.ToString()),
        //       new Claim("User", security.UserEmail),
        //       new Claim(ClaimTypes.Name, security.Name),
        //       new Claim(ClaimTypes.Role, RolesJson, JsonClaimValueTypes.JsonArray)
        //    };

        //    //Paylod

        //    var payload = new JwtPayload
        //    (
        //        _configuration["Authentication:Issuer"],
        //         _configuration["Authentication:Audience"],
        //        claims,
        //        DateTime.Now,
        //        DateTime.UtcNow.AddDays(10)
        //    );

        //  //  DateTime.UtcNow.AddMinutes(10)

        //    //sign
        //    var token = new JwtSecurityToken(header, payload);

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}


        private string GenerateToken(ResponseLogin security)
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

            string[,] hola = new string[2, security.ProgramXUsers.Count];

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

            security.ProgramXUsers.ForEach(x =>
            {
                lstRoles.Add(
                   new roles()
                   {
                       //ProgramId = x.ProgramId,
                       //RoleId = x.RoleId
                       RoleName = x.RoleName
                   }
              );
            });


            //var claims = new List<Claim>();
            //claims.Add(new Claim("Id", security.Id.ToString()));




            String RolesJson = JsonConvert.SerializeObject(lstRoles);


            //ORIGINAL VERSION LARGA
            //var claims = new[]
            //{
            //   new Claim("Id", security.Id.ToString()),
            //   new Claim("User", security.UserEmail),
            //   new Claim(ClaimTypes.Name, security.Name),
            //   new Claim(ClaimTypes.Role, RolesJson, JsonClaimValueTypes.JsonArray)
            //};


            //original 2
            //var claims = new[]
            //{
            //   new Claim("Id", security.Id.ToString()),
            //   new Claim("User", security.UserEmail),
            //   new Claim(ClaimTypes.Name, security.Name),
            //   new Claim(ClaimTypes.Role, "Consumer")
            //};


            //usando version corta para roles
            //ESTA FUNCIONA

            //var claims = new[]
            //{
            //    new Claim("Id", security.Id.ToString()),
            //    new Claim("User", security.UserEmail),
            //    new Claim(ClaimTypes.Name, security.Name),
            //    new Claim("role", "Consumer")
            //};


            var claims = new List<Claim>
           {
                new Claim("Id", security.Id.ToString()),
                new Claim("User", security.UserEmail),
                new Claim(ClaimTypes.Name, security.Name)
              
            };

            // claims

            security.ProgramXUsers.ForEach(x =>
            {
                claims.Add(new Claim(ClaimTypes.Role, x.RoleName));
            });


            //foreach (var roleName in rolesDelUsuario)
            //{
            //    // Usamos el ClaimTypes.Role estándar (URL larga)
            //    claims.Add(new Claim(ClaimTypes.Role, roleName));

            //    // (Opcional, pero recomendado para compatibilidad)
            //    claims.Add(new Claim("role", roleName));
            //}


            ////original 2
            //var claims = new[]
            //{
            //   new Claim("Id", security.Id.ToString()),
            //   new Claim("User", security.UserEmail),
            //   new Claim(ClaimTypes.Name, security.Name),

            //   new Claim("role", RolesJson,  JsonClaimValueTypes.JsonArray)

            //  // new Claim(ClaimTypes.Role, RolesJson,  JsonClaimValueTypes.JsonArray)
            //};


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


        //[HttpPost]
        //public async Task<IActionResult> ForgottenPassword(string email)
        //{
        //    //if is user valido
        //    var validation = await IsValidUser(login);
        //    if (validation.Item1)
        //    {
        //        var token = GenerateToken(validation.Item2);

        //        var response = new ApiResponses<string>(token, validation.Item2.ResponseCode);

        //        return Ok(response);
        //    }

        //    return NotFound();
        //}


    }

    public class roles
    { 
        //public int ProgramId { get; set; }

        public string RoleName { get; set; }


    }

 

}
