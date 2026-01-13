using LearnLinQWeb.Data;
using LearnLinQWeb.Data.Interfaces.User;
using LearnLinQWeb.Domain.Entities;
using LearnLinQWeb.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LearnLinQWeb.Services
{
    public class UserService : IUserService
    {
        private readonly IUserQuery _query;
        private readonly IUserCommand _command;

        public UserService(IUserQuery query, IUserCommand command)
        {
            _query = query;
            _command = command;
        }

        public List<User> GetAllUsers()
        {
            return _query.Query().ToList();
        }

        public User? GetUserById(int id)
        {
            return _query.Query().FirstOrDefault(u => u.Id == id);
        }

        public User? GetUserByUsername(string username)
        {
            return _query.Query().FirstOrDefault(u => u.Username == username);
        }

        public bool AddUser()
        {
            return false;
        }

        public bool DeleteUser()
        {
            return false;
        }

        public bool UpdateUser()
        {
            return false;
        }
    }
}
