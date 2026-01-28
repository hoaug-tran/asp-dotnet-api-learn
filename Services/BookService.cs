using System.Linq;
using LearnLinQWeb.Data;
using LearnLinQWeb.Data.Interfaces;
using LearnLinQWeb.Data.Interfaces.Book;
using LearnLinQWeb.Domain.Entities;
using LearnLinQWeb.DTOs.Common;
using LearnLinQWeb.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnLinQWeb.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public PagedResult<Book> GetAllBooks(int page, int limit, string? search, string? sortBy, string? order)
        {
            var query = _unitOfWork.BookQuery.Query();

            // filter and
            //if (!string.IsNullOrWhiteSpace(title))
            //    query = query.Where(b => b.Title.Contains(title));

            //if (!string.IsNullOrWhiteSpace(author))
            //    query = query.Where(b => b.Author.Contains(author));

            // filter or

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(b => b.Title.Contains(search) || b.Author.Contains(search));
            }

            int totalItems = query.Count();

            // sort
            string sort = sortBy?.ToLowerInvariant() ?? string.Empty;
            string ord = order?.ToLowerInvariant() ?? string.Empty;

            bool isDesc = ord == "desc";

            query = sort switch
            {
                "title" => isDesc
                    ? query.OrderByDescending(b => b.Title)
                    : query.OrderBy(b => b.Title),

                "author" => isDesc
                    ? query.OrderByDescending(b => b.Author)
                    : query.OrderBy(b => b.Author),

                "price" => isDesc
                    ? query.OrderByDescending(b => b.Price)
                    : query.OrderBy(b => b.Price),

                _ => query.OrderBy(b => b.Id)
            };


            // paging
            page = page < 1 ? 1 : page;
            limit = limit < 1 ? 10 : limit;

            int totalPages = (int)Math.Ceiling(totalItems / (double)limit);
            if (page > totalPages && totalPages > 0)
            {
                page = totalPages;
            }

            var items = query
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToList();

            return new PagedResult<Book>
            {
                Items = items,
                Page = page,
                Limit = limit,
                TotalItems = totalItems,
                TotalPages = totalPages,
                HasNext = page < totalPages,
                HasPrevious = page > 1
            };
        }


        public Book? GetBookById(int id)
        {
            return _unitOfWork.BookQuery.Query().FirstOrDefault(b => b.Id == id);
        }

        public bool AddBook(Book book)
        {
            _unitOfWork.BookCommand.Add(book);
            return _unitOfWork.SaveChanges() > 0;
        }

        public bool UpdateBook(int id, Book updatedBook)
        {
            var existBook = GetBookById(id);
            if (existBook != null)
            {
                existBook.Title = updatedBook.Title;
                existBook.Author = updatedBook.Author;
                existBook.Price = updatedBook.Price;

                _unitOfWork.BookCommand.Update(existBook);

                return _unitOfWork.SaveChanges() > 0;
            }

            return false;
        }

        public bool DeleteBook(int id)
        {
            var book = GetBookById(id);
            if (book != null)
            {
                _unitOfWork.BookCommand.Delete(book);
                return _unitOfWork.SaveChanges() > 0;
            }

            return false;
        }
    }   
}
