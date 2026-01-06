using LearnLinQWeb.Models;
using LearnLinQWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace LearnLinQWeb.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService;

        public BooksController(BookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_bookService.GetAllBooks());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id) // Nên đặt tên rõ ràng hơn là Get
        {
            var book = _bookService.GetBookById(id);
            return book != null ? Ok(book) : NotFound("Không tìm thấy sách");
        }

        [HttpPost]
        public IActionResult Create(Book book)
        {
            return (_bookService.AddBook(book))
                ? Ok("Thêm thành công")
                : BadRequest("Lỗi khi thêm sách");
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Book book)
        {
            return (_bookService.UpdateBook(id, book))
                ? Ok("Cập nhật sách thành công")
                : BadRequest("Lỗi khi cập nhật sách hoặc không tìm thấy ID");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return (_bookService.DeleteBook(id))
                ? Ok("Xoá sách thành công")
                : BadRequest("Xoá sách thất bại (ID không tồn tại)");
        }
    }
}