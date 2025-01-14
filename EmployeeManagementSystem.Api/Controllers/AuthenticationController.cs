using EmployeeManagementSystem.Application.Dtos;
using EmployeeManagementSystem.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserAccountService _userAccountService;

        public AuthenticationController(IUserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateAsync(RegisterDto? user)
        {
            if (user == null)
            {
                return BadRequest("Model is empty");
            }

            var result = await _userAccountService.CreateAsync(user);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> CreateAsync(LoginDto? user)
        {
            if (user == null)
            {
                return BadRequest("Model is empty");
            }

            var result = await _userAccountService.SignInAsync(user);

            return Ok(result);
        }
    }
}
