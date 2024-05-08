using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalk.API.Models.DTOs;
using NZWalk.API.Repositories;
using System.Text.RegularExpressions;

namespace NZWalk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }
        // POST: /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.UserName,
                Email = registerRequestDto.UserName
            };

            var identityResult = await _userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if (identityResult.Succeeded)
            {
                // Add role to this User
                if(registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {

                    identityResult =  await _userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
                    if (identityResult.Succeeded)
                    {
                        return Ok("User was registered! Please Login.");
                    }
                }
            }
            return BadRequest("Something went wrong!");
        }

        //post /api/Auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDto.UserName);
            if(user != null)
            {
                var checkPasswordResult = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

                if (checkPasswordResult)
                {

                    //Get current user roles
                    var userRoles = await _userManager.GetRolesAsync(user);
                    if( userRoles != null )
                    {
                       var jwtToken = _tokenRepository.CreateJWTToken(user, userRoles.ToList());
                        var response = new LoginReponseDto
                        {
                            JwtToken = jwtToken
                        };
                        return Ok(response);
                    }
                    //Create Token
                    return Ok();
                }
            }
            return BadRequest("Username or Password is incorrect");
        }
    }
}
