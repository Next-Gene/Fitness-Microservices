using System;
using System.Threading;
using System.Threading.Tasks;
using Fitness.Features.AssignFitnessPlanCommand;
using FitnessCalculationService.Data;
using FitnessCalculationService.Data.Enums;
using FitnessCalculationService.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace FitnessCalculationService.Tests.Features.AssignFitnessPlanCommand
{
    public class AssignFitnessPlanCommandHandlerTests
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Mock<IRepository<FitnessPlanConfigdb>> _repoMock;
        private readonly AssignFitnessPlanCommandHandler _handler;

        public AssignFitnessPlanCommandHandlerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _repoMock = new Mock<IRepository<FitnessPlanConfigdb>>();

            _handler = new AssignFitnessPlanCommandHandler(_dbContext, _repoMock.Object);
        }

        [Fact]
        public async Task Handle_UserStatsNotFound_ThrowsException()
        {
            // Arrange
            var command = new Fitness.Features.AssignFitnessPlanCommand.AssignFitnessPlanCommand(Guid.NewGuid());

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("User fitness stats not found.");
        }

        [Fact]
        public async Task Handle_NoMatchingConfig_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var activity = new WeightGoalActivitydb
            {
                Id = userId,
                UserId = userId,
                Goal = Goal.LoseWeight
            };
            
            var stat = new UserFitnessStatdb
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                weightGoalActivity = activity,
                Status = "Normal",
                CalorieTarget = 2000,
                InsertDate = DateTime.UtcNow
            };

            await _dbContext.WeightGoalActivity.AddAsync(activity);
            await _dbContext.UserFitnessStat.AddAsync(stat);
            await _dbContext.SaveChangesAsync();

            var command = new Fitness.Features.AssignFitnessPlanCommand.AssignFitnessPlanCommand(userId);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("No matching fitness plan configuration found.");
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsPlanId()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var activity = new WeightGoalActivitydb
            {
                Id = userId,
                UserId = userId,
                Goal = Goal.LoseWeight
            };
            
            var stat = new UserFitnessStatdb
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                weightGoalActivity = activity,
                Status = "Normal",
                CalorieTarget = 2000,
                InsertDate = DateTime.UtcNow
            };

            var planId = Guid.NewGuid();
            var config = new FitnessPlanConfigdb
            {
                Id = Guid.NewGuid(),
                Goal = "LoseWeight",
                Status = "Normal",
                MinCalorie = 1500,
                MaxCalorie = 2500,
                WorkoutPlan = new WorkoutPlandb { PlanId = planId }
            };

            await _dbContext.WeightGoalActivity.AddAsync(activity);
            await _dbContext.UserFitnessStat.AddAsync(stat);
            await _dbContext.FitnessPlanConfig.AddAsync(config);
            await _dbContext.SaveChangesAsync();

            var command = new Fitness.Features.AssignFitnessPlanCommand.AssignFitnessPlanCommand(userId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(planId);
        }
    }
}
