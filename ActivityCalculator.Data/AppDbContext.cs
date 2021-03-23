using ActivityCalculator.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ActivityCalculator.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ActivityLog>()
                .HasOne(al => al.Dataset)
                .WithMany(ds => ds.ActivityLogs)
                .HasForeignKey(al => al.DatasetId);
        }

        public DbSet<ActivityDataset> ActivityDatasets { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }

        private void OnSaveChanges()
        {
            var utcNow = DateTime.UtcNow;
            foreach (var entityAccessor in ChangeTracker.Entries())
            {
                if (entityAccessor.State == EntityState.Added && entityAccessor.Entity is Entity entity)
                {
                    entity.CreatedOn = utcNow;
                }
            }
        }

        public override int SaveChanges()
        {
            OnSaveChanges();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            OnSaveChanges();
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
