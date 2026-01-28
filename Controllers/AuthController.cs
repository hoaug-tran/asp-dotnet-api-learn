using AutoMapper;
using LearnLinQWeb.Application.Interfaces;
using LearnLinQWeb.Domain.Entities;
using LearnLinQWeb.DTOs.Auth;
using LearnLinQWeb.Services;
using LearnLinQWeb.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LearnLinQWeb.Controllers
{
    [Route("/api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IJwtService _jwtService;

        public AuthController(IAuthService service, IUserService userService, IMapper mapper, IRefreshTokenService refreshTokenService, IJwtService jwtService)
        {
            _service = service;
            _mapper = mapper;
            _userService = userService;
            _refreshTokenService = refreshTokenService;
            _jwtService = jwtService;
        }

        //[HttpPost("login")]
        //public IActionResult Login([FromBody] LoginRequest? rq)
        //{
        //    if (rq == null ||
        //        string.IsNullOrWhiteSpace(rq.Username) ||
        //        string.IsNullOrWhiteSpace(rq.Password))
        //    {
        //        return BadRequest(new { message = "Dữ liệu không hợp lệ" });
        //    }

        //    var user = _service.LoginAsync(rq.Username, rq.Password);

        //    if (user != null)
        //    {
        //        var response = _mapper.Map<LoginResponse>(user);

        //        return Ok(new
        //        {
        //            message = "Đăng nhập thành công",
        //            data = response
        //        });
        //    }

        //    return Unauthorized(new { message = "Tài khoản hoặc mật khẩu không đúng" });
        //}
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest? rq)
        {
            if (rq == null || string.IsNullOrWhiteSpace(rq.Username) || string.IsNullOrWhiteSpace(rq.Password))
            {
                return BadRequest("Invalid request");
            }

            string username = rq.Username.ToLower().Trim();
            string password = rq.Password.Trim();

            try
            {
                var token = await _service.LoginAsync(username, password);
                var user = await _userService.GetUserByUsernameAsync(username);

                var refreshToken = _refreshTokenService.GenerateRefreshToken();
                var expirationDate = DateTime.UtcNow.AddDays(7);
                await _refreshTokenService.SaveRefreshTokenAsync(user.Id, refreshToken, expirationDate);

                return Ok(new
                {
                    message = "Đăng nhập thành công",
                    accessToken = token,
                    refreshToken = refreshToken,
                    tokenType = "Bearer",
                    expiresIn = 30 * 60,
                    user = new
                    {
                        id = user.Id,
                        name = user.Name,
                        username = user.Username,
                        role = user.Role
                    }
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Tài khoản hoặc mật khẩu không đúng" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest? rq)
        {
            if (rq == null || string.IsNullOrWhiteSpace(rq.Username) || string.IsNullOrWhiteSpace(rq.Password))
            {
                return BadRequest("Invalid request");
            }

            if (!string.Equals(rq.Password, rq.VerifyPassword))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Mật khẩu không khớp, vui lòng nhập lại !"
                });
            }

            string name = rq.Name.Trim();
            string username = rq.Username.ToLower().Trim();
            string password = rq.Password.Trim();

            var result = await _service.RegisterAsync(name, username, password);

            if (!result.Success)
            {
                return BadRequest(new
                {
                    success = result.Success,
                    message = result.Message
                });
            }

            return Ok(new
            {
                success = result.Success,
                message = result.Message
            });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest? rq)
        {
            if (rq == null || string.IsNullOrWhiteSpace(rq.RefreshToken))
            {
                return BadRequest("Invalid refresh token request");
            }

            try
            {
                var user = User;
                if (user == null || !user.Identity?.IsAuthenticated == true)
                {
                    return Unauthorized("User not authenticated");
                }

                var userId = int.Parse(user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
                var isValid = await _refreshTokenService.ValidateRefreshTokenAsync(userId, rq.RefreshToken);

                if (!isValid)
                {
                    return Unauthorized("Invalid refresh token");
                }

                var userData = await _userService.GetUserByIdAsync(userId);
                if (userData == null)
                {
                    return Unauthorized("User not found");
                }

                var newAccessToken = _jwtService.GenerateToken(userData);
                var newRefreshToken = _refreshTokenService.GenerateRefreshToken();
                var expirationDate = DateTime.UtcNow.AddDays(7);

                await _refreshTokenService.RevokeRefreshTokenAsync(userId, rq.RefreshToken);
                await _refreshTokenService.SaveRefreshTokenAsync(userId, newRefreshToken, expirationDate);

                return Ok(new
                {
                    accessToken = newAccessToken,
                    refreshToken = newRefreshToken,
                    tokenType = "Bearer",
                    expiresIn = 30 * 60
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest? rq)
        {
            if (rq == null)
            {
                return BadRequest("Invalid request");
            }

            try
            {
                var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
                await _refreshTokenService.RevokeRefreshTokenAsync(userId, rq.RefreshToken);

                return Ok(new { message = "Logout successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
