using LearnLinQWeb.Domain.Entities;
using LearnLinQWeb.DTOs.Common;

namespace LearnLinQWeb.Services.Interfaces;

public interface IBookService
{
    PagedResult<Book> GetAllBooks(int page, int limit, string? search, string? sortBy, string? order);

    Book? GetBookById(int id);

    bool AddBook(Book book);

    bool UpdateBook(int id, Book updatedBook);

    bool DeleteBook(int id);

}
