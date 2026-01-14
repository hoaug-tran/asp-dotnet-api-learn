using AutoMapper;
using LearnLinQWeb.DTOs;
using LearnLinQWeb.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LearnLinQWeb.Controllers
{
    [Route("/api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        private readonly IMapper _mapper;

        public AuthController(IAuthService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest? rq)
        {
            if (rq == null ||
                string.IsNullOrWhiteSpace(rq.Username) ||
                string.IsNullOrWhiteSpace(rq.Password))
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ" });
            }

            var user = _service.Login(rq.Username, rq.Password);

            if (user != null)
            {
                var response = _mapper.Map<LoginResponse>(user);

                return Ok(new
                {
                    message = "Đăng nhập thành công",
                    data = response
                });
            }

            return Unauthorized(new { message = "Tài khoản hoặc mật khẩu không đúng" });
        }
    }
}
