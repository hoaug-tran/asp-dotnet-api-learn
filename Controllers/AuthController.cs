using AutoMapper;
using LearnLinQWeb.Domain.Entities;
using LearnLinQWeb.DTOs;
using LearnLinQWeb.Services;
using LearnLinQWeb.Services.Interfaces;
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

        public AuthController(IAuthService service, IUserService userService, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
            _userService = userService;
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

            var token = await _service.LoginAsync(rq.Username, rq.Password);

            var user = await _userService.GetUserByUsernameAsync(rq.Username);

            return Ok(new
            {
                message = "Đăng nhập thành công",
                accessToken = token,
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

            var result = await _service.RegisterAsync(rq.Name, rq.Username, rq.Password);

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
    }
}
