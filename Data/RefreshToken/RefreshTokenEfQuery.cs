using LearnLinQWeb.Data.Interfaces.RefreshToken;
using LearnLinQWeb.Domain.Entities;
using LearnLinQWeb.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LearnLinQWeb.Data.RefreshToken;

public class RefreshTokenEfQuery : IRefreshTokenQuery
{
    private readonly AppDbContext _db;

    public RefreshTokenEfQuery(AppDbContext db)
    {
        _db = db;
    }

    public IQueryable<Domain.Entities.RefreshToken> Query()
    {
        return _db.RefreshTokens.AsNoTracking();
    }

    public async Task<Domain.Entities.RefreshToken?> GetByTokenAsync(string token)
    {
        return await _db.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(rt => rt.Token == token);
    }

    public async Task<List<Domain.Entities.RefreshToken>> GetByUserIdAsync(int userId)
    {
        return await _db.RefreshTokens
            .AsNoTracking()
            .Where(rt => rt.UserId == userId)
            .ToListAsync();
    }
}
