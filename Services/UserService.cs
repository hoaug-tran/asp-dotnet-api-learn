using LearnLinQWeb.Application.Interfaces;
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
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public List<User> GetAllUsers()
        {
            return _unitOfWork.UserQuery.Query().ToList();
        }

        public User? GetUserById(int id)
        {
            return _unitOfWork.UserQuery.Query().FirstOrDefault(u => u.Id == id);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return _unitOfWork.UserQuery.Query().FirstOrDefault(u => u.Username == username);
        }

        public async Task<bool> AddUserAsync(User user, string plainPassword)
        {
            user.PasswordHash = _passwordHasher.Hash(plainPassword);
            _unitOfWork.UserCommand.AddUser(user);

            return await _unitOfWork.SaveChangesAsync() > 0;
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
