using LearnLinQWeb.Data;
using LearnLinQWeb.Models;

namespace LearnLinQWeb.Services
{
    public class AuthService
    {

        private readonly MyDbContext _db;

        public AuthService(MyDbContext db)
        {
            _db = db;
        }

        public User? Login(string username,string password)
        {

            return _db.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

    }
}
