using GarmentStreet.Models;
using Microsoft.EntityFrameworkCore;

namespace GarmentStreet.DataAccess
{
    public class ApplicationDbContext : DbContext
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

    }
}
