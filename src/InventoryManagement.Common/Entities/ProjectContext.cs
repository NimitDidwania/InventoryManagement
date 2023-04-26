using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Common.Entities
{
    public class ProjectContext:DbContext
    {
        public ProjectContext(DbContextOptions option) : base(option)
        {
            
        }
        public DbSet<ProductDbo> Products{get;set;}
        public DbSet<OrderDbo>Orders{get;set;}
        public DbSet<UserDbo>Users{get;set;}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductDbo>()
                .HasKey(b => b.ProductId);
            modelBuilder.Entity<OrderDbo>()
                .HasKey(b => b.OrderId);
            modelBuilder.Entity<UserDbo>()
                .HasKey(b => b.UserId);

        } 
    }
}