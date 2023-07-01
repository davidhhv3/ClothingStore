using AutoMapper;
using ClothingStore.Api.Responses;
using ClothingStore.Core.DTOs;
using ClothingStore.Core.Entities;
using ClothingStore.Core.Interfaces;
using ClothingStore.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ClothingStore.Api.Controllers
{
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

        /// <summary>
        /// Create a new security user
        /// </summary>    
        /// <param name="securityDto">Security user data</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<SecurityDto>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Post(SecurityDto securityDto)
        {
            Security security = _mapper.Map<Security>(securityDto);
            if(security.Password != null)            
               security.Password = _passwordService.Hash(security.Password);
            await _securityService.RegisterUser(security);
            securityDto = _mapper.Map<SecurityDto>(security);
            ApiResponse<SecurityDto> response = new ApiResponse<SecurityDto>(securityDto);
            return Ok(response);
        }
    }

}
