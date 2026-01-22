using LearnLinQWeb.Domain.Entities;

namespace LearnLinQWeb.Data.Interfaces.User;

public interface IUserCommand
{
    void AddUser(Domain.Entities.User user);
    bool UpdateUser();
    bool DeleteUser();
    int SaveChanges();

}
