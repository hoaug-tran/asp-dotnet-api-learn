using LearnLinQWeb.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = LearnLinQWeb.DTOs.LoginRequest;

namespace LearnLinQWeb.Controllers
{
    [Route("/api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authServices;

        public AuthController(AuthService authService)
        {
            _authServices = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest rq)
        {
            var user = _authServices.Login(rq.Username, rq.Password);

            if (user!=null)
            {
                return Ok(new
                {
                    message = "Đăng nhập thành công",
                    username = user.Username,
                    role = user.Role 
                });
            }

            return Unauthorized(new {message = "Tài khoản hoặc mật khẩu không đúng" });
        }
    }


}
