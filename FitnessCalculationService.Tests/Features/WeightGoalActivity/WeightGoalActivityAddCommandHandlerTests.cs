using System;
using System.Threading;
using System.Threading.Tasks;
using FitnessCalculationService.Data;
using FitnessCalculationService.Data.Enums;
using FitnessCalculationService.Features.Dtos;
using FitnessCalculationService.Features.WeightGoalActivity.Comands;
using FitnessCalculationService.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace FitnessCalculationService.Tests.Features.WeightGoalActivity
{
    public class WeightGoalActivityAddCommandHandlerTests
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Mock<IRepository<WeightGoalActivitydb>> _repoMock;
        private readonly WeightGoalActivityAddCommandHandler _handler;

        public WeightGoalActivityAddCommandHandlerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _repoMock = new Mock<IRepository<WeightGoalActivitydb>>();

            _handler = new WeightGoalActivityAddCommandHandler(_dbContext, _repoMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_AddsAndReturnsId()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var dto = new AddWGA
            {
                UserId = userId,
                Age = 25,
                Gender = "F",
                Weight = 65,
                Height = 170,
                ActivityLevel = ActivityLevel.Advance,
                Goal = Goal.GetFitter
            };

            var command = new WeightGoalActivityAddComand(dto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repoMock.Verify(x => x.AddAsync(It.Is<WeightGoalActivitydb>(
                w => w.UserId == userId &&
                     w.Age == 25 &&
                     w.Gender == "F" &&
                     w.Weight == 65 &&
                     w.Height == 170
            )), Times.Once);

            _repoMock.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
