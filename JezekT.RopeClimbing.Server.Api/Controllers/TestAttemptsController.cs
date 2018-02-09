using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using JezekT.RopeClimbing.Domain.Entities;
using JezekT.RopeClimbing.Server.Services.TestAttempts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace JezekT.RopeClimbing.Server.Api.Controllers
{
    [Authorize("RopeClimbingApiOnly")]
    [Route("api/[controller]/[action]")]
    public class TestAttemptsController : Controller
    {
        private readonly TestAttemptServices _attemptServices;


        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> CreateAttempt([FromBody] JObject obj)
        {
            if (obj != null)
            {
                var racerName = obj["RacerName"].ToObject<string>();
                var timeInMiliseconds = obj["TimeInMiliseconds"].ToObject<int>();
                var attemptEndTime = obj["AttemptEndTime"].ToObject<DateTime>();

                if (racerName != null && attemptEndTime != default(DateTime))
                {
                    await _attemptServices.CreateAttemptAsync(new TestAttempt
                    {
                        RacerName = racerName,
                        TimeInMiliseconds = timeInMiliseconds,
                        AttemptEndTime = attemptEndTime
                    });
                    return Ok();
                }
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<List<TestAttempt>> Last100Attempts()
        {
            return await _attemptServices.GetLast100AttemptsAsync();
        }


        public TestAttemptsController(TestAttemptServices attemptServices)
        {
            if (attemptServices == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            _attemptServices = attemptServices;
        }
    }
}
