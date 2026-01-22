using LearnLinQWeb.Data.Interfaces.Auth;
using LearnLinQWeb.Domain.Entities;
using LearnLinQWeb.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LearnLinQWeb.Data;

public class AuthEfQuery : IAuthQuery
{
    private readonly AppDbContext _db;

    public AuthEfQuery(AppDbContext db)
    {
        _db = db;
    }

    public IQueryable<User> Query()
    {
        return _db.Users.AsNoTracking();
    }

    public Task<User?> GetByUsernameAsync(string username)
    {
        return _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == username);
    }
}
