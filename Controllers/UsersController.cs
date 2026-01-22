using LearnLinQWeb.Data.Interfaces;
using LearnLinQWeb.Domain.Entities;
using LearnLinQWeb.Services;
using LearnLinQWeb.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnLinQWeb.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(UserService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAllUsers());
        }

        //public bool AddUser(User user)
        //{
        //    _unitOfWork.UserCommand.Add(user);
        //    return _unitOfWork.SaveChanges() > 0;
        //}

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
