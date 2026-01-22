using LearnLinQWeb.Data.Interfaces.User;
using LearnLinQWeb.Domain.Entities;
using LearnLinQWeb.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LearnLinQWeb.Data;

public class UserEfQuery : IUserQuery
{
    private readonly AppDbContext _db;

    public UserEfQuery(AppDbContext db)
    {
        _db = db;
    }

    public IQueryable<User> Query()
    {
        return _db.Users.AsNoTracking();
    }
}
