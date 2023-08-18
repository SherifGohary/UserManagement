using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UserManagement.API.DataAccess.Entities;

namespace UserManagement.API.DataAccess
{
    public class UserManagementContext : IdentityDbContext<User>
    {
        private readonly IConfiguration _config;
        public UserManagementContext(IConfiguration config)
        {
            _config = config;
        }

        public DbSet<Address> Address { get; set; }
        public DbSet<Company> Company { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);

            builder.UseSqlServer(_config.GetConnectionString("UserManagementConnectionString"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Address)
                .WithOne()
                .HasForeignKey<User>(u => u.AddressId)
                .IsRequired(false);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Company)
                .WithOne()
                .HasForeignKey<User>(u => u.CompanyId)
                .IsRequired(false);

        }
    }
}
