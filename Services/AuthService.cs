using LearnLinQWeb.Data;
using LearnLinQWeb.Data.Interfaces;
using LearnLinQWeb.Data.Interfaces.Auth;
using LearnLinQWeb.Data.Interfaces.User;
using LearnLinQWeb.Domain.Entities;
using LearnLinQWeb.Services.Interfaces;

namespace LearnLinQWeb.Services
{
    public class AuthService : IAuthService
    {

        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public User? Login(string username,string password)
        {

            return _unitOfWork.AuthQuery.Query().FirstOrDefault(u => u.Username == username && u.Password == password);
        }

    }
}
