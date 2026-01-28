using LearnLinQWeb.Data.Interfaces.RefreshToken;
using LearnLinQWeb.Domain.Entities;
using LearnLinQWeb.Infrastructure.Persistence;

namespace LearnLinQWeb.Data.RefreshToken;

public class RefreshTokenEfCommand : IRefreshTokenCommand
{
    private readonly AppDbContext _db;

    public RefreshTokenEfCommand(AppDbContext db)
    {
        _db = db;
    }

    public void AddRefreshToken(Domain.Entities.RefreshToken refreshToken)
    {
        _db.RefreshTokens.Add(refreshToken);
    }

    public void DeleteRefreshToken(Domain.Entities.RefreshToken refreshToken)
    {
        _db.RefreshTokens.Remove(refreshToken);
    }

    public void DeleteRefreshTokensByUserId(int userId)
    {
        var tokens = _db.RefreshTokens.Where(rt => rt.UserId == userId).ToList();
        _db.RefreshTokens.RemoveRange(tokens);
    }

    public int SaveChanges()
    {
        return _db.SaveChanges();
    }
}
