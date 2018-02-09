using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JezekT.RopeClimbing.Domain.Entities;
using Newtonsoft.Json;

namespace JezekT.RopeClimbing.Api.Client.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var httpClient = new RopeClimbingHttpClient("", "", "", "", new ConsoleLogger());
            while (true)
            {
                var attempt = GetTestAttemptFromUser();
                var postResponse = await httpClient.PostAsync("api/testattempts/createattempt", attempt);
                if (postResponse != null)
                {
                    var attemptsResponse = await httpClient.GetResponseAsync("api/testattempts/last100attempts");
                    if (attemptsResponse != null)
                    {
                        var attempts = GetTestAttemptsFromResponse(attemptsResponse);
                        foreach (var testAttempt in attempts)
                        {
                            Console.WriteLine($"Id={testAttempt.Id};RacerName={testAttempt.RacerName};Time={testAttempt.TimeInMiliseconds / 1000}s;TimeStamp={testAttempt.AttemptEndTime}");
                        }
                    }
                }
            }
        }

        private static TestAttempt GetTestAttemptFromUser()
        {
            while (true)
            {
                var racerName = GetRacerNameFromUser();
                var timeInMiliseconds = GetTimeInMilisecondsFromUser();
                return new TestAttempt
                {
                    RacerName = racerName,
                    TimeInMiliseconds = timeInMiliseconds,
                    AttemptEndTime = DateTime.Now
                };
            }
        }

        private static string GetRacerNameFromUser()
        {
            while (true)
            {
                Console.Write("Enter racer name:");
                var racerName = Console.ReadLine();
                if (!string.IsNullOrEmpty(racerName))
                {
                    return racerName;
                }
            }
        }

        private static int GetTimeInMilisecondsFromUser()
        {
            while (true)
            {
                Console.Write("Enter attempt time in miliseconds:");
                var timeString = Console.ReadLine();
                if (int.TryParse(timeString, out var time))
                {
                    return time;
                }
                Console.WriteLine("Invalid time");
            }
        }

        private static List<TestAttempt> GetTestAttemptsFromResponse(string response)
        {
            try
            {
                return JsonConvert.DeserializeObject<List<TestAttempt>>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
