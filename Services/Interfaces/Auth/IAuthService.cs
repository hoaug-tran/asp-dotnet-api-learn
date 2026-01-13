using LearnLinQWeb.Domain.Entities;

namespace LearnLinQWeb.Services.Interfaces;

public interface IAuthService
{

    public User? Login(string username, string password);
}
