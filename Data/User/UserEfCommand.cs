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

    public void UpdateUser(User user)
    {
        var existingEntity = _db.Users.Local.FirstOrDefault(u => u.Id == user.Id);
        
        if (existingEntity != null)
        {
            _db.Entry(existingEntity).CurrentValues.SetValues(user);
        }
        else
        {
            _db.Attach(user);
            _db.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
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
