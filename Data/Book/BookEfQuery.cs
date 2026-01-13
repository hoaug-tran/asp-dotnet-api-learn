using LearnLinQWeb.Data.Interfaces.Book;
using LearnLinQWeb.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearnLinQWeb.Data;

public class BookEfQuery : IBookQuery
{
    private readonly AppDbContext _db;

    public BookEfQuery(AppDbContext db)
    {
        _db = db;
    }

    public IQueryable<Book> Query()
    {
        return _db.Books.AsNoTracking();
    }
}
