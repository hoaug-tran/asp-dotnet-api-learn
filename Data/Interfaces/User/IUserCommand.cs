using LearnLinQWeb.Domain.Entities;

namespace LearnLinQWeb.Data.Interfaces.User;

public interface IUserCommand
{
    bool AddUser();
    bool UpdateUser();
    bool DeleteUser();
    int SaveChanges();

}
