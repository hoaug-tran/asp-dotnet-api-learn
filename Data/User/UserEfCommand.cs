using LearnLinQWeb.Data.Interfaces.User;
using LearnLinQWeb.Domain.Entities;
using LearnLinQWeb.Infrastructure.Persistence;

namespace LearnLinQWeb.Data;

public class UserEfCommand : IUserCommand
{
    private readonly AppDbContext _db;

    public UserEfCommand(AppDbContext db)
    {
        _db = db;
    }

    public void AddUser(User user)
    {
        _db.Users.Add(user);
    }

    public bool UpdateUser()
    {
        return false;
    }

    public bool DeleteUser()
    {
        return false;
    }

    public int SaveChanges()
    {
        return _db.SaveChanges();
    }
}
