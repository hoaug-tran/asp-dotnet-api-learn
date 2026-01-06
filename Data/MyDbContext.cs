using LearnLinQWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnLinQWeb.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
    }
}
