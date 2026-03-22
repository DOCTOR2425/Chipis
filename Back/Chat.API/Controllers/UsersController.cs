using Chipis.API.DTOs;
using Chipis.Application.Abstractions;
using Chipis.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Chipis.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly Infrastructure.Options.CookieOptions _options;

        public UsersController(IUsersService usersService, IOptions<Infrastructure.Options.CookieOptions> options)
        {
            _usersService = usersService;
            _options = options.Value;
        }

        [AllowAnonymous]
        [HttpGet("getAll")]
        public async Task<ActionResult<List<UserResponse>>> GetAllUsers()
        {
            List<User> users = await _usersService.GetAllUsers();

            return Ok(users
                .Select(u => new UserResponse(u.UserId, u.Nickname, u.HashPassword))
                .ToList());
        }

        [Authorize]
        [HttpGet("testAuth")]
        public async Task<IActionResult> TestAuth()
        {
            return Ok("work");
        }

        [HttpGet("testException")]
        public async Task<IActionResult> TestException()
        {
            throw new InvalidOperationException("bruh");
            return Ok("work");
        }
    }
}
