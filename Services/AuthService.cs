using LearnLinQWeb.Data;
using LearnLinQWeb.Models;

namespace LearnLinQWeb.Services
{
    public class AuthService
    {

        private readonly AppDbContext _db;

        public AuthService(AppDbContext db)
        {
            _db = db;
        }

        public User? Login(string username,string password)
        {

            return _db.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

    }
}
