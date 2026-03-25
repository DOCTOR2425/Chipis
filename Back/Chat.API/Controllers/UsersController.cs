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

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [AllowAnonymous]
        [HttpGet("getAll")]
        public async Task<ActionResult<List<UserResponse>>> GetAllUsers()
        {
            List<User> users = await _usersService.GetAllUsers();

            return Ok(users
                .Select(u => new UserResponse(u.UserId, u.Nickname))
                .ToList());
        }

        [Authorize]
        [HttpGet("searchUser/{userName}")]
        public async Task<IActionResult> GetUser(string userName)
        {
            List<User> users = await _usersService.SearchUsersByNickname(userName);

            return Ok(users
                .Select(u => new UserResponse(u.UserId, u.Nickname))
                .ToList());
        }
    }
}
