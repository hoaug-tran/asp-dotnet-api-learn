using LearnLinQWeb.Domain.Entities;

namespace LearnLinQWeb.Data.Interfaces.User;

public interface IUserCommand
{
    void AddUser(Domain.Entities.User user);
    void UpdateUser(Domain.Entities.User user);
    bool DeleteUser();
    int SaveChanges();

}
