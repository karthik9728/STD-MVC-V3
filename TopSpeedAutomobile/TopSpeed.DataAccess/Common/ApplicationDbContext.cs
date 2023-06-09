using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TopSpeed.Domain.Common;
using TopSpeed.Domain.ModelAggregate.User;
using TopSpeed.Domain.Models;

namespace TopSpeed.DataAccess.Common
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        public DbSet<VehicleType> VehicleType { get; set; }

        public DbSet<Brand> Brand { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            foreach (var entity in base.ChangeTracker.Entries<BaseModel>()
                .Where(x=>x.State == EntityState.Added || x.State == EntityState.Modified))
            {

                if(entity.State == EntityState.Added)
                {
                    entity.Entity.CreatedOn = DateTime.UtcNow;
                }

                if(entity.State == EntityState.Modified)
                {
                    entity.Entity.ModifiedOn = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
