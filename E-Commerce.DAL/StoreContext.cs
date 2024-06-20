
using E_Commerce.DAL.Entities;
using E_Commerce.DAL.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
namespace E_Commerce.DAL
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options)
            : base(options)
        {
        }

        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Product> Products { get; set; }

        //El 7agat De msh lazm 3sahn ana 3amlha Configuration
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //de btnady ay 7aga betimplement da IEntityTypeConfiguration
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); 


        }
        // Your DbSet properties and other configurations here
    }
}
