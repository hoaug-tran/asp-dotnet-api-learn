using LearnLinQWeb.Data;
using LearnLinQWeb.Models;

namespace LearnLinQWeb.Services
{
    public class UserService
    {
        private readonly MyDbContext _db;

        public UserService(MyDbContext db)
        {
            _db = db;
        }

        public List<User> GetAllUsers()
        {
            return _db.Users.ToList();
        }

        public User? GetUserById(int id)
        {
            return _db.Users.FirstOrDefault(u => u.Id == id);
        }

        public User? GetUserByUsername(string username)
        {
            return _db.Users.FirstOrDefault(u => u.Username.Equals(username));
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
