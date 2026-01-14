using LearnLinQWeb.Data;
using LearnLinQWeb.Data.Interfaces;
using LearnLinQWeb.Data.Interfaces.User;
using LearnLinQWeb.Domain.Entities;
using LearnLinQWeb.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LearnLinQWeb.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<User> GetAllUsers()
        {
            return _unitOfWork.UserQuery.Query().ToList();
        }

        public User? GetUserById(int id)
        {
            return _unitOfWork.UserQuery.Query().FirstOrDefault(u => u.Id == id);
        }

        public User? GetUserByUsername(string username)
        {
            return _unitOfWork.UserQuery.Query().FirstOrDefault(u => u.Username == username);
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
