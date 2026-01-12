using System.Linq;
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

        public List<Book> GetAllBooks(int? page, int? limit, string? title, string? author, string? sortBy, string? order)
        {
            // AsQueryable -> "query này là query động -> chưa chạy -> vẫn còn build tiếp"
            var query = _db.Books.AsNoTracking().AsQueryable();

            // tìm kiếm, filter
            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(b => b.Title.Contains(title));
            }

            if (!string.IsNullOrWhiteSpace(author))
            {
                query = query.Where(b => b.Author.Contains(author));
            }

            // sắp xếp theo title hoặc author
            bool isDesc = order?.ToLower() == "desc";
            query = sortBy?.ToLower() switch
            {
                "title" => isDesc ? query.OrderByDescending(b => b.Title) : query.OrderBy(b => b.Title),
                "author" => isDesc ? query.OrderByDescending(b => b.Author) : query.OrderBy(b => b.Author),
                _ => query.OrderBy(b => b.Id)
            };

            /*
             sort phụ -> option
            "title" => isDesc
                ? query.OrderByDescending(b => b.Title).ThenBy(b => b.Id)
                : query.OrderBy(b => b.Title).ThenBy(b => b.Id),

            "author" => isDesc
                ? query.OrderByDescending(b => b.Author).ThenBy(b => b.Id)
                : query.OrderBy(b => b.Author).ThenBy(b => b.Id),
            */

            // phân trang, limit. PHÂN TRANG PHẢI CÓ ORDERBY !!!!
            if (page.HasValue && limit.HasValue)
            {
                int pageSize = limit.Value < 1 ? 10 : limit.Value;
                int currentPage = page.Value < 1 ? 1 : page.Value;

                query = query.Skip((currentPage - 1) * pageSize).Take(pageSize);
            }

            // chạy query
            return query.ToList();
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
