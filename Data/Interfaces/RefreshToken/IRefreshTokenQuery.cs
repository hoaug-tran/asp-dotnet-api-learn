using LearnLinQWeb.Domain.Entities;

namespace LearnLinQWeb.Data.Interfaces.RefreshToken;

public interface IRefreshTokenQuery
{
    IQueryable<Domain.Entities.RefreshToken> Query();
    Task<Domain.Entities.RefreshToken?> GetByTokenAsync(string token);
    Task<List<Domain.Entities.RefreshToken>> GetByUserIdAsync(int userId);
}
