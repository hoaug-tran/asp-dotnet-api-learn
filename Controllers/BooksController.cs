using AutoMapper;
using Azure.Core;
using LearnLinQWeb.Data.Interfaces;
using LearnLinQWeb.Domain.Entities;
using LearnLinQWeb.DTOs;
using LearnLinQWeb.DTOs.Common;
using LearnLinQWeb.Services;
using LearnLinQWeb.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnLinQWeb.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _service;
        private readonly IMapper _mapper;

        public BooksController(IBookService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll(int page, int limit, string? title, string? author, string? sortBy, string? order)
        {
            var books = _service.GetAllBooks(page, limit, title, author, sortBy, order);
            var res = new PagedResult<BookResponse>
            {
                Items = _mapper.Map<List<BookResponse>>(books.Items),
                Page = books.Page,
                Limit = books.Limit,
                TotalItems = books.TotalItems,
                TotalPages = books.TotalPages,
                HasNext = books.HasNext,
                HasPrevious = books.HasPrevious
            };

            return Ok(new
            {
                data = res,
                message = "OK"
            });
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id) 
        {
            var book = _service.GetBookById(id);
            if (book == null)
            {
                return NotFound(new { message = "Không tìm thấy sách" });
            }

            var response = _mapper.Map<BookResponse>(book);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create([FromBody] CreateBookRequest request)
        {
            var book = _mapper.Map<Book>(request);
            return (_service.AddBook(book)) ? Ok(new { message = "Thêm thành công" }) : BadRequest(new { message = "Lỗi khi thêm sách" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateBookRequest request)
        {
            var book = _mapper.Map<Book>(request);
            return (_service.UpdateBook(id, book)) ? Ok(new { message = "Cập nhật sách thành công" }) : BadRequest(new { message = "Lỗi khi cập nhật sách hoặc không tìm thấy ID" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return (_service.DeleteBook(id)) ? Ok(new { message = "Xoá sách thành công" }) : BadRequest(new { message = "Xoá sách thất bại (ID không tồn tại)" });
        }
    }
}
