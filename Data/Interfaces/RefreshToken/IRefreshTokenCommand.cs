using LearnLinQWeb.Domain.Entities;

namespace LearnLinQWeb.Data.Interfaces.RefreshToken;

public interface IRefreshTokenCommand
{
    void AddRefreshToken(Domain.Entities.RefreshToken refreshToken);
    void DeleteRefreshToken(Domain.Entities.RefreshToken refreshToken);
    void DeleteRefreshTokensByUserId(int userId);
    int SaveChanges();
}
