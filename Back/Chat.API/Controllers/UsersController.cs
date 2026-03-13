using Chipis.API.DTOs;
using Chipis.Application.Abstractions;
using Chipis.Core.Models;
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

        [HttpGet("getAll")]
        public async Task<ActionResult<List<UserResponse>>> GetAllUsers()
        {
            List<User> users = await _usersService.GetAllUsers();

            return Ok(users
                .Select(u => new UserResponse(u.UserId, u.Name, u.HashPassword))
                .ToList());
        }

        [HttpPost("createUser")]
        public async Task<ActionResult<Guid>> CreateUser(
            [FromBody] UserRequest request)
        {
            User user = new User(Guid.NewGuid(), request.Name, request.HashPassword);

            return Ok(await _usersService.CreateUser(user));
        }
    }
}
