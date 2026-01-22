using LearnLinQWeb.Domain.Entities;

namespace LearnLinQWeb.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);

}
