using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentSupervisorService.Models.Response.UserResponse;
using StudentSupervisorService.Models;
using StudentSupervisorService.Service;
using Domain.Entity;
using Infrastructures.Interfaces.IUnitOfWork;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentSupervisorAPI.Controllers
{
    [Route("api/auths")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly LoginService _loginService;

        public AuthenticationController(LoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var (success, message, token) = await _loginService.Login(login, false);

            if (success)
            {
                return Ok(new { token });
            }

            return Unauthorized(new { message });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { message = "Token is required." });
            }

            _loginService.Logout(token);
            return Ok(new { message = "Logged out successfully." });
        }
    }

}
