using LearnLinQWeb.Domain.Entities;

namespace LearnLinQWeb.Services.Interfaces;

public interface IUserService
{

    List<User> GetAllUsers();
    User? GetUserById(int id);

    Task<User?> GetUserByUsernameAsync(string username);
    //bool AddUser(User user);
    //bool UpdateUser(User user);
    //bool DeleteUser(int id);

    Task<bool> AddUserAsync(User user, string plainPassword);
}

