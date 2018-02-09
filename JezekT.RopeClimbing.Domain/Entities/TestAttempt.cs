using System;

namespace JezekT.RopeClimbing.Domain.Entities
{
    public class TestAttempt
    {
        public int Id { get; set; }
        public string RacerName { get; set; }
        public int TimeInMiliseconds { get; set; }
        public DateTime AttemptEndTime { get; set; }
    }
}
