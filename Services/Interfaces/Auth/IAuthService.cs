using LearnLinQWeb.Domain.Entities;

namespace LearnLinQWeb.Services.Interfaces;

public interface IAuthService
{

    User? Login(string username, string password);
}
