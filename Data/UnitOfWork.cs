using LearnLinQWeb.Data.Interfaces;
using LearnLinQWeb.Data.Interfaces.Auth;
using LearnLinQWeb.Data.Interfaces.Book;
using LearnLinQWeb.Data.Interfaces.User;

namespace LearnLinQWeb.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;
    private IBookQuery _bookQuery;
    private IBookCommand _bookCommand;
    private IUserQuery _userQuery;
    private IUserCommand _userCommand;
    private IAuthQuery _authQuery;

    public UnitOfWork(AppDbContext db)
    {
        _db = db;
    }

    public IBookQuery BookQuery => _bookQuery ??= new BookEfQuery(_db);

    public IBookCommand BookCommand => _bookCommand ??= new BookEfCommand(_db);

    public IUserQuery UserQuery => _userQuery ??= new UserEfQuery(_db);

    public IUserCommand UserCommand => _userCommand ??= new UserEfCommand(_db);

    public IAuthQuery AuthQuery => _authQuery ??= new AuthEfQuery(_db);

    public int SaveChanges() => _db.SaveChanges();

    public async Task<int> SaveChangesAsync() => await _db.SaveChangesAsync();

    public void Dispose() => _db?.Dispose();
}
