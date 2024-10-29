using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Robbu.Desafio.Jean.API.Models.Requests;
using Robbu.Desafio.Jean.API.Models.Responses;
using Robbu.Desafio.Jean.API.Persistence.Entities;
using Robbu.Desafio.Jean.API.Services;
using System.ComponentModel.DataAnnotations;

namespace Robbu.Desafio.Jean.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtService _jwtService;

        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IJwtService jwtService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> PostLogin([FromBody, Required] PostLoginRequest requestBody)
        {
            var user = await _userManager.FindByEmailAsync(requestBody.Email);
            if (user == null)
            {
                return Unauthorized();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, requestBody.Password!, false);
            if (!result.Succeeded)
            {
                return Unauthorized();
            }
            var token = await _jwtService.GenerateTokenAsync(user);

            var response = new ApiResponse<TokenResponse>("Token gerado com sucesso", token);

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> PostRegister([FromBody, Required] RegisterRequest requestBody)
        {
            var existingUser = await _userManager.FindByEmailAsync(requestBody.Email);
            if (existingUser != null)
            {
                return Conflict("Email já registrado.");
            }

            var user = new IdentityUser
            {
                UserName = requestBody.Email,
                Email = requestBody.Email
            };

            var result = await _userManager.CreateAsync(user, requestBody.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var token = await _jwtService.GenerateTokenAsync(user);

            var response = new ApiResponse<TokenResponse>("Token gerado com sucesso", token);

            return Ok(response);
        }
    }
}