using Microsoft.EntityFrameworkCore;
using ItemProcessorApp.Models;

namespace ItemProcessorApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

     public DbSet<ItemProcessorApp.Models.Item> Items { get; set; }
    }
}