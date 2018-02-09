using JezekT.RopeClimbing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JezekT.RopeClimbing.Server.Services.Data.EntityConfigs
{
    public class TestAttemptConfiguration : IEntityTypeConfiguration<TestAttempt>
    {
        public void Configure(EntityTypeBuilder<TestAttempt> builder)
        {
            builder.ToTable("TestAttemptSet");
            builder.Property(x => x.RacerName).HasMaxLength(100).IsRequired();
        }
    }
}
