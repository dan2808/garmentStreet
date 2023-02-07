using GarmentStreet.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GarmentStreet.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Target> Targets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Variation> Variations { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<VariationOption> VariationOptions  { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }



    }
}
