using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using JezekT.RopeClimbing.Domain.Entities;
using Newtonsoft.Json;

namespace JezekT.RopeClimbing.Api.Client
{
    public class RopeClimbingClient
    {
        private readonly RopeClimbingHttpClient _httpClient;


        public async Task<bool> AddTestAttempt(TestAttempt testAttempt)
        {
            if (testAttempt == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            return await _httpClient.PostAsync("api/testattempts/createattempt", testAttempt) != null;
        }

        public async Task<List<TestAttempt>> GetLast100TestAttempts()
        {
            var attemptsResponse = await _httpClient.GetResponseAsync("api/testattempts/last100attempts");
            return GetObjectFromResponse<List<TestAttempt>>(attemptsResponse);
        }


        public RopeClimbingClient(RopeClimbingHttpClient httpClient)
        {
            if (httpClient == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            _httpClient = httpClient;
        }


        private T GetObjectFromResponse<T>(string response) where T : class
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
