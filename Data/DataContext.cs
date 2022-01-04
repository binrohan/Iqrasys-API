using iqrasys.api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace iqrasys.api.Data
{
    public class DataContext : IdentityDbContext<User, Role, string,  IdentityUserClaim<string>,
    UserRole, IdentityUserLogin<string>,
    IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Message> Messages { get; set; }
        public DbSet<DemoRequest> DemoRequests { get; set; }
        public DbSet<Solution> Solutions { get; set; }
        public DbSet<ArchiveUser> ArchiveUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // builder.Entity<Role>()
            //     .Property(r => r.Id)
            //     .ValueGeneratedOnAdd();


            // builder.Entity<User>(b =>
            // {
            //     b.HasMany<UserRole>(u => u.UserRoles)
            //         .WithOne(ur => ur.User)
            //         .HasForeignKey(ur => ur.UserId)
            //         .IsRequired();
            // });

            // builder.Entity<Role>(b =>
            // {
            //     b.HasMany<UserRole>(r => r.UserRoles)
            //         .WithOne(ur => ur.Role)
            //         .HasForeignKey(ur => ur.RoleId)
            //         .IsRequired();
            // });
        }
    }
}