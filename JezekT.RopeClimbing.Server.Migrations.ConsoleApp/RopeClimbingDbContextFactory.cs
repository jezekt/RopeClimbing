using System.IO;
using JezekT.RopeClimbing.Server.Services.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace JezekT.RopeClimbing.Server.Migrations.ConsoleApp
{
    public class RopeClimbingDbContextFactory : IDesignTimeDbContextFactory<RopeClimbingDbContext>
    {
        public RopeClimbingDbContext CreateDbContext(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = configurationBuilder.Build();

            var builder = new DbContextOptionsBuilder<RopeClimbingDbContext>();
            builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly(configuration.GetValue<string>("MigrationAssembly")));
            return new RopeClimbingDbContext(builder.Options);
        }
    }
}
