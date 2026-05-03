using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using ProgressTrackingService.Data;
using ProgressTrackingService.Features.LogWeight;
using ProgressTrackingService.Models;
using Xunit;

namespace ProgressTrackingService.Tests.Features.LogWeight
{
    public class LogWeightHandlerTests
    {
        private readonly ProgressDbContext _dbContext;
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly IMemoryCache _cache;
        private readonly LogWeightHandler _handler;

        public LogWeightHandlerTests()
        {
            var options = new DbContextOptionsBuilder<ProgressDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            _dbContext = new ProgressDbContext(options);
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _cache = new MemoryCache(new MemoryCacheOptions());

            _handler = new LogWeightHandler(_dbContext, _httpClientFactoryMock.Object, _cache);
        }

        [Fact]
        public async Task Handle_FirstTimeLoggingWeight_CreatesStatistics()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new LogWeightCommand(userId, 80.5m, DateTimeOffset.UtcNow, Guid.NewGuid().ToString());

            // Mock HttpClient for the internal call (even if it's commented out in handler logic, it's still called)
            _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            
            var stats = await _dbContext.UserStatistics.FirstOrDefaultAsync(s => s.UserId == userId);
            stats.Should().NotBeNull();
            stats.StartingWeight.Should().Be(80.5m);
            stats.CurrentWeight.Should().Be(80.5m);

            var entry = await _dbContext.WeightEntries.FirstOrDefaultAsync(e => e.UserId == userId);
            entry.Should().NotBeNull();
            entry.WeightKg.Should().Be(80.5m);
        }

        [Fact]
        public async Task Handle_ExistingUser_UpdatesStatistics()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var stats = new UserStatistics
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                StartingWeight = 90.0m,
                CurrentWeight = 85.0m
            };
            await _dbContext.UserStatistics.AddAsync(stats);
            await _dbContext.SaveChangesAsync();

            var command = new LogWeightCommand(userId, 82.0m, DateTimeOffset.UtcNow, Guid.NewGuid().ToString());
            _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient());

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            var updatedStats = await _dbContext.UserStatistics.FirstAsync(s => s.UserId == userId);
            updatedStats.StartingWeight.Should().Be(90.0m);
            updatedStats.CurrentWeight.Should().Be(82.0m);
        }
    }
}
