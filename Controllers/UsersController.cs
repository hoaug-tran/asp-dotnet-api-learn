using System.Collections.Generic;
using AutoMapper;
using LearnLinQWeb.Application.Interfaces;
using LearnLinQWeb.Data.Interfaces;
using LearnLinQWeb.Domain.Entities;
using LearnLinQWeb.DTOs.Books;
using LearnLinQWeb.DTOs.Common;
using LearnLinQWeb.DTOs.Users;
using LearnLinQWeb.Services;
using LearnLinQWeb.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Reflection.Metadata.BlobBuilder;

namespace LearnLinQWeb.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IMapper _mapper;
        private readonly IFileUploadService _fileUploadService;

        public UsersController(IUserService service, IMapper mapper, IFileUploadService fileUploadService)
        {
            _service = service;
            _mapper = mapper;
            _fileUploadService = fileUploadService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetAll(int page = 1, int limit = 10, string? search = null, string? sortBy = null, string? order = null)
        {
            var users = _service.GetAllUsers(page, limit, search, sortBy, order);

            var res = new PagedResult<UserResponse>
            {
                Items = _mapper.Map<List<UserResponse>>(users.Items),
                Page = users.Page,
                Limit = users.Limit,
                TotalItems = users.TotalItems,
                TotalPages = users.TotalPages,
                HasNext = users.HasNext,
                HasPrevious = users.HasPrevious

            };

            return Ok(new { data = res, message = "OK" });


        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateUserRequest rq, IFormFile? avatar)
        {
            // 🔍 DEBUG: Log avatar parameter
            Console.WriteLine($"========== USER CREATE DEBUG ==========");
            Console.WriteLine($"📨 Avatar parameter: {(avatar == null ? "NULL" : $"File({avatar.FileName}, {avatar.Length} bytes)")}");
            Console.WriteLine($"📨 Request fields: name={rq?.Name}, username={rq?.Username}, role={rq?.Role}");
            Console.WriteLine($"=====================================");

            if (rq == null || string.IsNullOrWhiteSpace(rq.Username) || string.IsNullOrWhiteSpace(rq.Password))
            {
                return BadRequest("Request không hợp lệ");
            }

            if (!string.Equals(rq.Password, rq.VerifyPassword))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Mật khẩu không khớp, vui lòng nhập lại !"
                });
            }

            var existingUser = await _service.GetUserByUsernameAsync(rq.Username.ToLower().Trim());
            if (existingUser != null)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Username đã tồn tại. Vui lòng chọn username khác!"
                });
            }

            string avatarUrl = rq.AvatarUrl;

            var user = new User
            {
                Name = rq.Name?.Trim(),
                Username = rq.Username.ToLower().Trim(),
                Email = rq.Email?.Trim(),
                Phone = rq.Phone?.Trim(),
                AvatarUrl = avatarUrl?.Trim(),
                Role = string.IsNullOrWhiteSpace(rq.Role) ? "User" : rq.Role
            };

            try
            {
                var result = await _service.AddUserAsync(user, rq.Password.Trim());

                if (!result)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Thêm người dùng thất bại"
                    });
                }

                var savedUser = await _service.GetUserByUsernameAsync(user.Username);

                if (avatar != null && avatar.Length > 0)
                {
                    try
                    {
                        Console.WriteLine($"📤 Uploading avatar for user {savedUser.Id}: {avatar.FileName} ({avatar.Length} bytes)");
                        avatarUrl = await _fileUploadService.UploadAvatarAsync(avatar, savedUser.Id);
                        Console.WriteLine($"✅ Avatar uploaded successfully: {avatarUrl}");

                        savedUser.AvatarUrl = avatarUrl;
                        await _service.UpdateUserAsync(savedUser);
                        Console.WriteLine($"✅ User updated with avatar URL");
                    }
                    catch (Exception uploadEx)
                    {
                        Console.WriteLine($"❌ Avatar upload error: {uploadEx.Message}");
                        Console.WriteLine($"❌ Stack trace: {uploadEx.StackTrace}");
                    }
                }
                else
                {
                    Console.WriteLine($"⚠️  No avatar to upload (avatar={avatar}, length={avatar?.Length})");
                }

                return Ok(new
                {
                    success = true,
                    message = "Thêm người dùng thành công",
                    data = _mapper.Map<UserResponse>(savedUser)
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ General error: {ex.Message}");
                Console.WriteLine($"❌ Stack trace: {ex.StackTrace}");
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        //public bool UpdateUser(User user)
        //{
        //    _unitOfWork.UserCommand.Update(user);
        //    return _unitOfWork.SaveChanges() > 0;
        //}

        //public bool DeleteUser(int id)
        //{
        //    var user = GetUserById(id);
        //    if (user != null)
        //    {
        //        _unitOfWork.UserCommand.Delete(user);
        //        return _unitOfWork.SaveChanges() > 0;
        //    }
        //    return false;
        //}
    }
}
