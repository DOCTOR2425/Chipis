using Chipis.API.DTOs;
using Chipis.Application.Abstractions;
using Chipis.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [HttpGet("searchUser/{nickname}")]
        public async Task<ActionResult<List<UserResponse>>> SearchUser(string nickname)
        {
            List<User> users = await _usersService.SearchUsersByNickname(nickname);

            return Ok(users
                .Select(u => new UserResponse(u.UserId, u.Nickname))
                .ToList());
        }
    }
}
