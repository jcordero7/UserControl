using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserControl.Api.Responses;
using UserControl.Core.DTOs;
using UserControl.Core.Entities;
using UserControl.Core.Enumerations;
using UserControl.Core.Interfaces;
using UserControl.Infrastructure.Interfaces;
using System.Threading.Tasks;
using UserControl.Application.Interfaces.services;
using UserControl.Application.Interfaces;
using UserControl.Application.Responses;

namespace UserControl.Api.Controllers
{

   // [Authorize(Roles = nameof(RoleType.Administrator))]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly ISecurityService _securityService;
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;

        public SecurityController(ISecurityService securityService, IMapper mapper, IPasswordService passwordService)
        {
            _securityService = securityService;
            _mapper = mapper;
            _passwordService = passwordService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(SecurityDto securityDto)
        {
            var security = _mapper.Map<Security>(securityDto);
            security.Password = _passwordService.Hash(security.Password);
            await _securityService.RegisterUser(security);

            securityDto = _mapper.Map<SecurityDto>(security);
            var response = new ApiResponses<SecurityDto>(securityDto);
            return Ok(response);
        }



    }
}
