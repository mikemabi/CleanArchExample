using CleanArchExample.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchExample.Infrastructure.Data
{
    public class MyDBContext : DbContext
    {
        public MyDBContext(DbContextOptions<MyDBContext> options) : base(options)
        {
        
        }
        public DbSet<Product> Products { get; set; }
    }
}
