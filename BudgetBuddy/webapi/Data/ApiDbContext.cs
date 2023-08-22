using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection.Metadata;
using webapi.Models;

namespace webapi.Data
{
    public class ApiDbContext:DbContext
    {
        public DbSet<Users> User { get; set; }
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) {
            
            try
            {
                var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                if (databaseCreator != null)
                {
                    // Create Database if cannot connect
                    if (!databaseCreator.CanConnect()) databaseCreator.Create();

                    // Create Tables if no tables exist
                    if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
    }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .HasIndex(b => b.Email)
                .IsUnique();

            modelBuilder.Entity<Users>()
            .Property(u => u.UserRole)
            .HasConversion<string>()
            .HasMaxLength(50);
        }
    }
    }

