using System.IdentityModel.Tokens.Jwt;
using IdentityServer4.AccessTokenValidation;
using JezekT.RopeClimbing.Server.Services.Data;
using JezekT.RopeClimbing.Server.Services.TestAttempts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace JezekT.RopeClimbing.Server.Api
{
    public class Startup
    {
        private readonly IConfigurationRoot _configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddTransient<TestAttemptServices>();
            services.AddDbContext<RopeClimbingDbContext>(options => options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = _configuration.GetValue<string>("IdentityServer:Authority");
                    options.RequireHttpsMetadata = _configuration.GetValue<bool>("IdentityServer:RequireHttpsMetadata");
                    options.ApiName = _configuration.GetValue<string>("IdentityServer:ApiName");
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RopeClimbingApiOnly", policy => policy.RequireClaim("scope", "ropeClimbingApiServerScope"));
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            if (env.IsDevelopment())
            {
                loggerFactory.AddConsole(_configuration.GetSection("Logging"));
                loggerFactory.AddDebug();
                loggerFactory.AddSerilog(new LoggerConfiguration().ReadFrom.Configuration(_configuration)
                    .WriteTo.Async(a => new LoggerConfiguration().ReadFrom.Configuration(_configuration), 500)
                    .CreateLogger(), true);
            }
            else
            {
                loggerFactory.AddSerilog(new LoggerConfiguration().ReadFrom.Configuration(_configuration)
                    .WriteTo.Async(a => new LoggerConfiguration().ReadFrom.Configuration(_configuration), 500)
                    .CreateLogger(), true);
            }

            app.UseAuthentication();
            app.UseMvc();
        }


        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);

            builder.AddEnvironmentVariables();
            _configuration = builder.Build();
        }
    }
}
