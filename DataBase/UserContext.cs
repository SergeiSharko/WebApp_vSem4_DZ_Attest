using System.Data;
using Microsoft.EntityFrameworkCore;
using WebApp_vSem4.Models;

namespace WebApp_vSem4.DataBase
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public UserContext() { }

        public UserContext(DbContextOptions<UserContext> dbContext) : base(dbContext) { }    
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var _dbConnectionString = "Server=localhost;Port=3306;Database=WebAppDB_vSem4;User=root;Password=12345;";
            optionsBuilder.LogTo(Console.WriteLine)                          
                          .UseMySql(_dbConnectionString, ServerVersion.AutoDetect(_dbConnectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasKey(x => x.Id)
                      .HasName("user_pk");

                entity.Property(x => x.Name)
                      .HasColumnName("name")
                      .HasMaxLength(25);

                entity.HasIndex(x => x.Name)
                      .IsUnique();

                entity.Property(x => x.Password)
                      .HasColumnName("password");

                entity.Property(x => x.Salt)
                      .HasColumnName("salt");

                entity.Property(x => x.RoleId)
                      .HasConversion<int>();

                entity.HasOne(x => x.Role)
                      .WithMany(x => x.Users)
                      .HasForeignKey(x => x.RoleId);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("role");

                entity.Property(x => x.RoleId)
                      .HasConversion<int>();

                entity.HasData(Enum.GetValues(typeof(RoleId))
                      .Cast<RoleId>()
                      .Select(x => new Role { RoleId = x, Name = x.ToString() }));
            });
        }
    }
}
