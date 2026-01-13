using LearnLinQWeb.Services;
using LearnLinQWeb.Services.Interfaces;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using LoginRequest = LearnLinQWeb.DTOs.LoginRequest;

namespace LearnLinQWeb.Controllers
{
    [Route("/api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest rq)
        {
            if (string.IsNullOrWhiteSpace(rq.Username) ||
                string.IsNullOrWhiteSpace(rq.Password))
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ" });
            }

            var user = _service.Login(rq.Username, rq.Password);

            if (user != null)
            {
                return Ok(new
                {
                    message = "Đăng nhập thành công",
                    username = rq.Username,
                    role = user.Role
                });
            }

            return Unauthorized(new {message = "Tài khoản hoặc mật khẩu không đúng" });
        }
    }


}
