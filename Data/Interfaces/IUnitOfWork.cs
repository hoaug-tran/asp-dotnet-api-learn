using LearnLinQWeb.Data.Interfaces.Auth;
using LearnLinQWeb.Data.Interfaces.Book;
using LearnLinQWeb.Data.Interfaces.User;

namespace LearnLinQWeb.Data.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IBookQuery BookQuery { get; }
    IBookCommand BookCommand { get; }
    IUserQuery UserQuery { get; }
    IUserCommand UserCommand { get; }
    IAuthQuery AuthQuery { get; }

    int SaveChanges();
    Task<int> SaveChangesAsync();
}
