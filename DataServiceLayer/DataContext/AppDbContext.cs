using DataModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataServiceLayer.DataContext
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }


        public DbSet<Category>? Categories { get; set; }
        public DbSet<Product>? Products { get; set; }

        public DbSet<AppUser>? AppUsers { get; set; }

    }
}
