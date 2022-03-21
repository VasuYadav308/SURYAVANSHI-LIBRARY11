using Microsoft.EntityFrameworkCore;
using SURYAVANSHI_LIBRARY.Models;

namespace SURYAVANSHI_LIBRARY.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Author> Author { get; set; }
        public DbSet<Publisher> Publisher { get; set; }
        public DbSet<Book> Book { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
    }
}
