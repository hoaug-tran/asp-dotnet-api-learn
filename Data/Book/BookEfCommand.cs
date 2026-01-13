using LearnLinQWeb.Data.Interfaces.Book;
using LearnLinQWeb.Domain.Entities;

namespace LearnLinQWeb.Data;

public class BookEfCommand : IBookCommand
{
    private readonly AppDbContext _db;

    public BookEfCommand(AppDbContext db)
    {
        _db = db;
    }

    public void Add(Book book)
    {
        _db.Books.Add(book);
    } 

    public void Update(Book book)
    {
        _db.Books.Update(book);
    }

    public void Delete(Book book)
    {
        _db.Books.Remove(book);
    }

    public int SaveChanges()
    {
        return _db.SaveChanges();
    }
}

