using LearnLinQWeb.Data;
using LearnLinQWeb.Data.Interfaces.Auth;
using LearnLinQWeb.Data.Interfaces.User;
using LearnLinQWeb.Domain.Entities;
using LearnLinQWeb.Services.Interfaces;

namespace LearnLinQWeb.Services
{
    public class AuthService : IAuthService
    {

        private readonly IAuthQuery _command;

        public AuthService(IAuthQuery command)
        {
            _command = command;
        }

        public User? Login(string username,string password)
        {

            return _command.Query().FirstOrDefault(u => u.Username == username && u.Password == password);
        }

    }
}
