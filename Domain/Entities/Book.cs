using System.ComponentModel.DataAnnotations.Schema;

namespace LearnLinQWeb.Domain.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; } 
        public decimal? Price { get; set; }

    }
}
