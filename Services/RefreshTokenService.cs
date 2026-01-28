using System.Security.Cryptography;
using LearnLinQWeb.Application.Interfaces;
using LearnLinQWeb.Data.Interfaces;
using LearnLinQWeb.Domain.Entities;

namespace LearnLinQWeb.Services;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly IUnitOfWork _unitOfWork;

    public RefreshTokenService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    public async Task<bool> SaveRefreshTokenAsync(int userId, string token, DateTime expirationDate)
    {
        try
        {
            var refreshToken = new RefreshToken
            {
                UserId = userId,
                Token = token,
                ExpirationDate = expirationDate
            };

            _unitOfWork.RefreshTokenCommand.AddRefreshToken(refreshToken);
            await _unitOfWork.SaveChangesAsync();
            
            Console.WriteLine($"Đã lưu refresh token cho user có ID: {userId}");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi SaveRefreshTokenAsync: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> ValidateRefreshTokenAsync(int userId, string token)
    {
        try
        {
            var refreshToken = await _unitOfWork.RefreshTokenQuery.GetByTokenAsync(token);

            if (refreshToken == null)
            {
                Console.WriteLine($"Không tìm thấy refresh token");
                return false;
            }

            if (refreshToken.UserId != userId)
            {
                Console.WriteLine($"Refresh token không thuộc về user có ID: {userId}");
                return false;
            }

            if (refreshToken.ExpirationDate < DateTime.UtcNow)
            {
                Console.WriteLine($"Refresh token đã hết hạn");
                return false;
            }

            Console.WriteLine($"Refresh token không hợp lệ");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi ValidateRefreshTokenAsync: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> RevokeRefreshTokenAsync(int userId, string token)
    {
        try
        {
            var refreshToken = await _unitOfWork.RefreshTokenQuery.GetByTokenAsync(token);

            if (refreshToken != null)
            {
                _unitOfWork.RefreshTokenCommand.DeleteRefreshToken(refreshToken);
                await _unitOfWork.SaveChangesAsync();
                Console.WriteLine($"Refresh token đã bị thu hồi");
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi RevokeRefreshTokenAsync error: {ex.Message}");
            return false;
        }
    }
}
