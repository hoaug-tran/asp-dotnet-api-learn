namespace LearnLinQWeb.DTOs;

public class CreateBookRequest
{
    public string? Title { get; set; }
    public string? Author { get; set; }
    public decimal? Price { get; set; }
}