using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LearnLinQWeb.Application.Interfaces;
using LearnLinQWeb.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace LearnLinQWeb.Infrastructure.Security;

public class JwtService : IJwtService
{
    private readonly IConfiguration _config;

    public JwtService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(User user)
    {
        var claims = new[] {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var secret = _config["Jwt:Key"];
        if (string.IsNullOrWhiteSpace(secret))
            throw new Exception("Thiếu JWT Key");

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secret)
        );

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)

        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateAccessToken(User user)
    {
        return GenerateToken(user);
    }
}
