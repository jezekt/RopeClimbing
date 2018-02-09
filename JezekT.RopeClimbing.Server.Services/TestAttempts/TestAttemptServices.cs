using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using JezekT.RopeClimbing.Domain.Entities;
using JezekT.RopeClimbing.Server.Services.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JezekT.RopeClimbing.Server.Services.TestAttempts
{
    public class TestAttemptServices
    {
        private readonly RopeClimbingDbContext _dbContext;
        private readonly ILogger _logger;


        public async Task CreateAttemptAsync(TestAttempt attempt)
        {
            if (attempt == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            try
            {
                _dbContext.TestAttemptSet.Add(attempt);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Create TestAttempt failed. {ex.GetBaseException()?.Message ?? ex.Message}");
                throw;
            }
        }

        public async Task<List<TestAttempt>> GetAttemptsAsync()
        {
            return await _dbContext.TestAttemptSet.OrderByDescending(x => x.Id).Take(100).ToListAsync();
        }


        public TestAttemptServices(RopeClimbingDbContext dbContext, ILogger<TestAttemptServices> logger)
        {
            if (dbContext == null || logger == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            _dbContext = dbContext;
            _logger = logger;
        }
    }
}
