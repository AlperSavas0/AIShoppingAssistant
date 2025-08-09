using Microsoft.EntityFrameworkCore;
using ShoppingAssistantAI.Models.Configurations;
using ShoppingAssistantAI.Models.Entities;

namespace ShoppingAssistantAI.Models.ContextClasses
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new AppUserConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
        }


        public DbSet<AppUser> AppUsers{ get; set; }
        public DbSet<Product> Products{ get; set; }
        public DbSet<Order> Orders{ get; set; }
        public DbSet<OrderDetail> OrderDetails{ get; set; }

    }
    
    
}
