using LearnLinQWeb.Domain.Entities;
using LearnLinQWeb.DTOs;

namespace LearnLinQWeb.Services.Interfaces;

public interface IAuthService
{

    //User? Login(string username, string password);
    Task<string> LoginAsync(string username, string password);

    Task<RegisterResponse> RegisterAsync(string name, string username, string password);
}
