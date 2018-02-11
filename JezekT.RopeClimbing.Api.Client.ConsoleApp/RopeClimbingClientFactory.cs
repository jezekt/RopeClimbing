using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace JezekT.RopeClimbing.Api.Client.ConsoleApp
{
    public class RopeClimbingClientFactory
    {
        private readonly RopeClimbingHttpClient _httpClient;


        public RopeClimbingClient Create()
        {
            return new RopeClimbingClient(_httpClient);
        }


        public RopeClimbingClientFactory()
        {
            var logger = new ConsoleLogger();
            try
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("httpClientConfig.json", false, true)
                    .Build();
                _httpClient = new RopeClimbingHttpClient(configuration.GetValue<string>("identityServerUrl"),
                    configuration.GetValue<string>("clientId"),
                    configuration.GetValue<string>("clientSecret"),
                    configuration.GetValue<string>("ropeClimbingApiUrl"), logger);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error while reading config file. {ex.GetBaseException().Message}");
                throw;
            }
        }
    }
}
