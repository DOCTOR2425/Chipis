using Chipis.API.DTOs;
using Chipis.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Chipis.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IAuthService _authService;
        private readonly Infrastructure.Options.CookieOptions _options;

        public AuthController(
            IUsersService usersService, 
            IAuthService authService,
            IOptions<Infrastructure.Options.CookieOptions> options)
        {
            _usersService = usersService;
            _authService = authService;
            _options = options.Value;
        }

        [HttpPost("registerUser")]
        public async Task<IActionResult> RegisterUser(
            [FromBody] RegisterUserRequest request)
        {
            var (accessToken, refreshToken) = await _authService
                .RegisterUser(request.Nickname, request.Telephone, request.Password);

            return Ok((accessToken, refreshToken));
        }

        [HttpPost("loginUser")]
        public async Task<ActionResult<string>> Login(
            [FromBody] LoginUserRequest request)
        {
            var (accessToken, refreshToken) = await _authService
                .Login(request.Telephone, request.Password);

            HttpContext.Response.Cookies.Append(_options.RefreshTokenName, refreshToken, new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddDays(_options.CookieLifetimeDays)
            });

            return Ok(accessToken);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Fefresh()
        {
            if (!HttpContext.Request.Cookies.TryGetValue(_options.RefreshTokenName, out var refreshToken))
                return Unauthorized("Refresh token missing");

            var result = await _authService.RefreshTokens(refreshToken);

            Response.Cookies.Append(
                _options.RefreshTokenName,
                result.refreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.Now.AddDays(_options.CookieLifetimeDays)
                });

            return Ok(new
            {
                accessToken = result.accessToken
            });
        }
    }
}
