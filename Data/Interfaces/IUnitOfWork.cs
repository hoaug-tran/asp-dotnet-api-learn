using LearnLinQWeb.Data.Interfaces.Auth;
using LearnLinQWeb.Data.Interfaces.Book;
using LearnLinQWeb.Data.Interfaces.User;
using LearnLinQWeb.Data.Interfaces.RefreshToken;

namespace LearnLinQWeb.Data.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IBookQuery BookQuery { get; }
    IBookCommand BookCommand { get; }
    IUserQuery UserQuery { get; }
    IUserCommand UserCommand { get; }
    IAuthQuery AuthQuery { get; }
    IRefreshTokenQuery RefreshTokenQuery { get; }
    IRefreshTokenCommand RefreshTokenCommand { get; }

    int SaveChanges();
    Task<int> SaveChangesAsync();
}
