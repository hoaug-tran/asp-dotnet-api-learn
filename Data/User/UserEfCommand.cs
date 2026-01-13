using LearnLinQWeb.Data.Interfaces.User;
using LearnLinQWeb.Domain.Entities;

namespace LearnLinQWeb.Data;

public class UserEfCommand : IUserCommand
{
    private readonly AppDbContext _db;

    public UserEfCommand(AppDbContext db)
    {
        _db = db;
    }

    public bool AddUser()
    {
        return false;
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
