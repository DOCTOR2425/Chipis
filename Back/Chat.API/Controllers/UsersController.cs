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

        [HttpPost("registerUser")]
        public async Task<IActionResult> RegisterUser(
            [FromBody] RegisterUserRequest request)
        {
            var (accessToken, refreshToken) = await _usersService
                .RegisterUser(request.Nickname, request.Telephone, request.Password);

            return Ok((accessToken, refreshToken));
        }

        //[HttpPost("loginUser")]
        //public async Task<ActionResult<(string, string)>> Login(
        //    [FromBody] LoginUserRequest request)
        //{
        //    var (accessToken, refreshToken) = await _usersService
        //        .Login(request.Telephone, request.Password);

        //    Console.WriteLine(accessToken);
        //    Console.WriteLine(refreshToken);

        //    return Ok((accessToken, refreshToken));
        //}

        [HttpPost("loginUser")]
        public async Task<ActionResult<string>> Login(
            [FromBody] LoginUserRequest request)
        {
            var (accessToken, refreshToken) = await _usersService
                .Login(request.Telephone, request.Password);

            HttpContext.Response.Cookies.Append(_options.RefreshToken, refreshToken, new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddDays(_options.CookieLifetimeDays)
            });

            return Ok(accessToken);
        }
    }
}
