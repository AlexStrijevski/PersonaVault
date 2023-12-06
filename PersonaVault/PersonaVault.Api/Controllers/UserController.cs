using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonaVault.Business.Managers;
using PersonaVault.Business.Security;
using PersonaVault.Contracts.DTOs;
using PersonaVault.Contracts.Mappers;
using PersonaVault.Contracts.Requests;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

namespace PersonaVault.Api.Controllers
{
    [Controller]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _userManager;
        private readonly IJwtService _jwtService;
        private readonly ILogger<UserController> _logger;

        public UserController(
            IUserManager userManager,
            IJwtService jwtService,
            ILogger<UserController> logger)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _logger = logger;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var response = await _userManager.RegisterUser(request);

            return StatusCode(response.StatusCode, response.Message);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _userManager.Login(request);
            if (!response.IsSuccess)
            {
                return StatusCode(response.StatusCode, response.ErrorMessage);
            }
            return Ok(_jwtService.GetJwtToken(response.Id, response.Role));
        }

        [Authorize]
        [HttpPost("GetFullUserData")]
        public async Task<IActionResult> GetFullUserData()
        {
            if (!Guid.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value, out var userId))
            {
                _logger.LogError($"Failed to retreive User Id from JWT Token");
                return StatusCode(500, "Failed to retrieve User Id");
            }

            var response = await _userManager.GetFullUserData(userId);

            if (response == null)
                return StatusCode(500, "Failed to get full user data");

            return StatusCode(200, response);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromBody][Required] string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest("Username cannot be empty");

            var response = await _userManager.DeleteUser(username);

            return StatusCode(response.StatusCode, response.Message);
        }
    }
}
