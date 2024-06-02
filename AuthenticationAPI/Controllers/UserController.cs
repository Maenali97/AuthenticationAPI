using AuthenticationAPI.Interfaces;
using AuthenticationAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserAppService _userAppService;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserAppService userAppService, ILogger<UserController> logger)
        {
            _userAppService = userAppService;
            _logger = logger;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel user)
        {
            var result = await _userAppService.LoginAsync(user);
            _logger.LogInformation("Login Successfully");
            return Ok(new { token  = result });
        }
        [Authorize]
        [HttpPost("addrole")]
        public async Task<IActionResult> AddRole(AddRoleModel model)
        {

            var result = await _userAppService.AddRoleAsync(model);

            return Ok(result);
        }

        [Authorize]
        [HttpPost("getusertoken")]
        public async Task<IActionResult> GetUserToken(string email, string password)
        {

            var result = await _userAppService.GetUserTokenAsync(email, password);

            return Ok(result);
        }
    }
}
