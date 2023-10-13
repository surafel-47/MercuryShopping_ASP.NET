using MercuryShopping.Models.Tables;
using Microsoft.EntityFrameworkCore;

namespace MercuryShopping.Models
{
    public class MyDBContext : DbContext
    {
     
        public MyDBContext(DbContextOptions<MyDBContext> opts) : base(opts)
        {

        }

        //This are the tables
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Catagory> Catagory { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Reviews> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                        .HasIndex(c => c.Email)
                        .IsUnique();

            modelBuilder.Entity<Customer>()
                        .HasIndex(c => c.PhoneNumber)
                        .IsUnique();

            modelBuilder.Entity<OrderDetails>()
                .HasKey(od => new { od.OrderID, od.ProID });
            
            base.OnModelCreating(modelBuilder);
        }

    }

}
