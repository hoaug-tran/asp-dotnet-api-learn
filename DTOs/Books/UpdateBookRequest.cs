namespace LearnLinQWeb.DTOs.Books;

public class UpdateBookRequest
{
    public string? Title { get; set; }
    public string? Author { get; set; }
    public decimal? Price { get; set; }
}
