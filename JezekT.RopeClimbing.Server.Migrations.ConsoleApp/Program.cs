using System;
using JezekT.RopeClimbing.Server.Services.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace JezekT.RopeClimbing.Server.Migrations.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var services = new ServiceCollection();
                services.AddTransient<IDesignTimeDbContextFactory<RopeClimbingDbContext>, RopeClimbingDbContextFactory>();
                var serviceProvider = services.BuildServiceProvider();

                var serviceFactory = serviceProvider.GetService<IDesignTimeDbContextFactory<RopeClimbingDbContext>>();

                Console.WriteLine("Creating DB context...");
                using (var context = serviceFactory.CreateDbContext(args))
                {
                    Console.WriteLine("Running migrate...");
                    context.Database.Migrate();
                }

                Console.WriteLine("Done.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();
        }
    }
}
