using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection.Metadata;
using webapi.Entities;

namespace webapi.Data
{
    public class ApiDbContext:DbContext
    {
        public DbSet<Users> User { get; set; }
        public DbSet<Expenses> Expenses { get; set; }
        public DbSet<webapi.Entities.Budget> Budget { get; set; } = default!;

        public ApiDbContext() { }
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) {
            
            try
            {
                var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                if (databaseCreator != null)
                {
                    Console.WriteLine("db created");
                    // Create Database if cannot connect
                    if (!databaseCreator.CanConnect()) databaseCreator.Create();

                    // Create Tables if no tables exist
                    //if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
                    Console.WriteLine("tables created");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
    }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=root123;database=testDb3");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .HasIndex(b => b.Email)
                .IsUnique();

            modelBuilder.Entity<Users>()
            .Property(u => u.UserRole)
            .HasConversion<string>()
            .HasMaxLength(50);

            modelBuilder.Entity<Users>()
                .HasOne(e => e.Budget)
                .WithOne()
                .HasForeignKey<Budget>(e => e.UserId)
                .IsRequired();

            

            modelBuilder.Entity<Expenses>()
        .HasOne(e => e.User)
        .WithMany(u => u.Expenses)
        .HasForeignKey(e => e.UserId)
        .IsRequired();



        
        }
    }
    }

