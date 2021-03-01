using Ikea.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Ikea.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<OrganizationalStructure> OrganizationalStructures { get; set; }
        public DbSet<Furniture> Furnitures { get; set; }
        public DbSet<Inventory> Inventories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeViewModel>().HasNoKey();

            modelBuilder.Entity<OrganizationalStructure>()
                .ToTable("OrganizationalStructure")
                .HasKey(p => p.Id);

            modelBuilder.Entity<OrganizationalStructure>()
                .Property(b => b.Name)
                .IsRequired();

            modelBuilder.Entity<Furniture>()
                .ToTable("Furniture")
                .HasKey(k => k.Id);

            modelBuilder.Entity<Furniture>()
               .HasOne<Inventory>(f => f.Inventory)
               .WithOne(p => p.Furniture)
               .HasForeignKey<Inventory>(p => p.ObjectId);

            modelBuilder.Entity<Inventory>()
                .ToTable("Inventory")
                .HasKey(p => p.Id);

            modelBuilder.Entity<Inventory>()
                .Property(b => b.ObjectId)
                .IsRequired();

            modelBuilder.Entity<Inventory>()
                .HasIndex(p => p.ObjectId)
                .IsUnique();
        }
    }
}
