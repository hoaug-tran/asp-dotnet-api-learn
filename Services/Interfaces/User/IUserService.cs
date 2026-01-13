using LearnLinQWeb.Domain.Entities;

namespace LearnLinQWeb.Services.Interfaces;

public interface IUserService
{

    public List<User>? GetAllUsers();

    public User? GetUserById(int id);

    public User? GetUserByUsername(string username);

    public bool AddUser();

    public bool UpdateUser();

    public bool DeleteUser();
}

