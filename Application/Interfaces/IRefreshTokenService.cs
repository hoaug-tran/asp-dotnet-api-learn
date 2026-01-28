namespace LearnLinQWeb.Application.Interfaces;

public interface IRefreshTokenService
{
    string GenerateRefreshToken();
    Task<bool> SaveRefreshTokenAsync(int userId, string token, DateTime expirationDate);
    Task<bool> ValidateRefreshTokenAsync(int userId, string token);
    Task<bool> RevokeRefreshTokenAsync(int userId, string token);
}
