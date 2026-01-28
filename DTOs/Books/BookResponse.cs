using System.ComponentModel.DataAnnotations.Schema;

namespace LearnLinQWeb.DTOs.Books;

public class BookResponse
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    [Column(TypeName = "decimal(18,0)")]
    public decimal? Price { get; set; }
}
