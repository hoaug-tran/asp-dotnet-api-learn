using LearnLinQWeb.Data;
using LearnLinQWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnLinQWeb.Services
{
    public class UserService
    {
        private readonly AppDbContext _db;

        public UserService(AppDbContext db)
        {
            _db = db;
        }

        public List<User> GetAllUsers()
        {
            return _db.Users.ToList();
        }

        public User? GetUserById(int id)
        {
            return _db.Users.AsNoTracking().FirstOrDefault(u => u.Id == id);
        }

        public User? GetUserByUsername(string username)
        {
            return _db.Users.AsNoTracking().FirstOrDefault(u => u.Username == username);
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
