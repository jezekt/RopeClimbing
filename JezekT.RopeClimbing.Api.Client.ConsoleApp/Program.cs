using System;
using System.Threading.Tasks;
using JezekT.RopeClimbing.Domain.Entities;

namespace JezekT.RopeClimbing.Api.Client.ConsoleApp
{
    public class Program
    {
        static async Task Main()
        {
            var factory = new RopeClimbingClientFactory();
            var client = factory.Create();
            while (true)
            {
                var attempt = GetTestAttemptFromUser();
                if (await client.AddTestAttempt(attempt))
                {
                    var attempts = await client.GetLast100TestAttempts();
                    foreach (var testAttempt in attempts)
                    {
                        Console.WriteLine($"Id={testAttempt.Id};RacerName={testAttempt.RacerName};Time={(decimal)testAttempt.TimeInMiliseconds / 1000}s;TimeStamp={testAttempt.AttemptEndTime}");
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

    }
}
