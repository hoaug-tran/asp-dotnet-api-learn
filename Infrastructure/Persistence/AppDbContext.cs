using LearnLinQWeb.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearnLinQWeb.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
