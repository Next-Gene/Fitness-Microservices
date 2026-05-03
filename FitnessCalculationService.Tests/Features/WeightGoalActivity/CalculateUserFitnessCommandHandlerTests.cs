using System;
using System.Threading;
using System.Threading.Tasks;
using FitnessCalculationService.Data;
using FitnessCalculationService.Data.Enums;
using FitnessCalculationService.Features.WeightGoalActivity.Comands;
using FitnessCalculationService.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace FitnessCalculationService.Tests.Features.WeightGoalActivity
{
    public class CalculateUserFitnessCommandHandlerTests
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Mock<IRepository<UserFitnessStatdb>> _repoMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly CalculateUserFitnessCommandHandler _handler;

        public CalculateUserFitnessCommandHandlerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _repoMock = new Mock<IRepository<UserFitnessStatdb>>();
            _configMock = new Mock<IConfiguration>();

            _handler = new CalculateUserFitnessCommandHandler(_dbContext, _repoMock.Object, _configMock.Object);
        }

        [Fact]
        public async Task Handle_UserExists_CalculatesFitnessAndSaves()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var profile = new WeightGoalActivitydb
            {
                Id = userId,
                Gender = "M",
                Weight = 80, // kg
                Height = 180, // cm
                Age = 30,
                ActivityLevel = ActivityLevel.Intermediate,
                Goal = Goal.LoseWeight
            };

            await _dbContext.WeightGoalActivity.AddAsync(profile);
            await _dbContext.SaveChangesAsync();

            var command = new CalculateUserFitnessCommand(userId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.UserId.Should().Be(userId);

            // BMR for Male: 10 * 80 + 6.25 * 180 - 5 * 30 + 5 = 800 + 1125 - 150 + 5 = 1780
            result.Bmr.Should().Be(1780);

            // TDEE: 1780 * 1.55 = 2759
            result.Tdee.Should().Be(2759);

            // Target (LoseWeight): 2759 - 500 = 2259
            result.CalorieTarget.Should().Be(2259);
            result.Status.Should().Be("Normal"); // <= 2500 -> Normal

            _repoMock.Verify(x => x.AddAsync(It.IsAny<UserFitnessStatdb>()), Times.Once);
            _repoMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Handle_UserDoesNotExist_ThrowsException()
        {
            // Arrange
            var command = new CalculateUserFitnessCommand(Guid.NewGuid());

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("User profile not found.");
        }
    }
}
