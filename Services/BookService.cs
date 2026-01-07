using LearnLinQWeb.Data;
using LearnLinQWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnLinQWeb.Services
{
    public class BookService
    {
        private readonly AppDbContext _db;

        public BookService(AppDbContext db)
        {
            _db = db;
        }

        public List<Book> GetAllBooks()
        {
            return _db.Books.AsNoTracking().ToList();
        }

        public Book? GetBookById(int id)
        {
            return _db.Books.AsNoTracking().FirstOrDefault(b => b.Id == id);
        }

        public bool AddBook(Book book)
        {
            _db.Books.Add(book);
            return _db.SaveChanges() > 0;
        }

        public bool UpdateBook(int id, Book updatedBook)
        {
            var existBook = GetBookById(id);
            if (existBook != null)
            {
                existBook.Title = updatedBook.Title;
                existBook.Author = updatedBook.Author;
                existBook.Price = updatedBook.Price;

                _db.Books.Update(existBook);

                return _db.SaveChanges() > 0;
            }
            return false;
        }

        public bool DeleteBook(int id)
        {
            var book = GetBookById(id);
            if (book != null)
            {
                _db.Remove(book);
                return _db.SaveChanges() > 0;
            }

            return false;
        }

    }   
}
