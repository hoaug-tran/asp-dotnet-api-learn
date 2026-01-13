using LearnLinQWeb.Domain.Entities;

namespace LearnLinQWeb.Data.Interfaces.Book;

public interface IBookCommand
{
    void Add(Domain.Entities.Book book);
    void Update(Domain.Entities.Book book);
    void Delete(Domain.Entities.Book book);
    int SaveChanges();
}
