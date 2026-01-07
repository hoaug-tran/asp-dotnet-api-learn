using System.ComponentModel.DataAnnotations.Schema;

namespace LearnLinQWeb.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        public Book()
        {

        }
        public Book(int id, string title, string author, decimal price)
        {
            Id = id;
            Title = title;
            Author = author;
            Price = price;
        }

    }
}
