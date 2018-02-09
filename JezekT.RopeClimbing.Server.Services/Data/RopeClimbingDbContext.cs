using JezekT.RopeClimbing.Domain.Entities;
using JezekT.RopeClimbing.Server.Services.Data.EntityConfigs;
using Microsoft.EntityFrameworkCore;

namespace JezekT.RopeClimbing.Server.Services.Data
{
    public class RopeClimbingDbContext : DbContext
    {
        public DbSet<TestAttempt> TestAttemptSet { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new TestAttemptConfiguration());
        }


        public RopeClimbingDbContext(DbContextOptions<RopeClimbingDbContext> options)
            : base(options)
        {
        }

    }
}
