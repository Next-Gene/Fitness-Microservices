using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Moq;
using WorkoutService.Contracts;
using WorkoutService.Domain.Entities;
using WorkoutService.Domain.Interfaces;
using WorkoutService.Features.Workouts.StartWorkoutSession;
using WorkoutService.Features.Workouts.StartWorkoutSession.Dtos;
using WorkoutService.Features.Workouts.StartWorkoutSession.ViewModels;
using WorkoutService.Infrastructure;
using WorkoutService.Infrastructure.Data;
using Xunit;

namespace WorkoutService.Tests.Features.Workouts
{
    public class StartWorkoutSessionCommandHandlerTests
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly BaseRepository<Workout> _workoutRepository;
        private readonly BaseRepository<WorkoutSession> _sessionRepository;
        private readonly Mock<IPublishEndpoint> _publishEndpointMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly StartWorkoutSessionCommandHandler _handler;

        public StartWorkoutSessionCommandHandlerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _workoutRepository = new BaseRepository<Workout>(_dbContext);
            _sessionRepository = new BaseRepository<WorkoutSession>(_dbContext);

            _publishEndpointMock = new Mock<IPublishEndpoint>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _handler = new StartWorkoutSessionCommandHandler(
                _publishEndpointMock.Object,
                _currentUserServiceMock.Object,
                _workoutRepository,
                _sessionRepository,
                _unitOfWorkMock.Object
            );
        }

        [Fact]
        public async Task Handle_UserNotAuthenticated_ReturnsFail()
        {
            // Arrange
            _currentUserServiceMock.Setup(x => x.IsAuthenticated).Returns(false);
            var command = new StartWorkoutSessionCommand(1, new StartWorkoutSessionDto { PlannedDuration = 30, Difficulty = "Beginner" });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("User is not authenticated");
        }

        [Fact]
        public async Task Handle_ValidRequest_StartsSessionAndPublishesEvent()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _currentUserServiceMock.Setup(x => x.IsAuthenticated).Returns(true);
            _currentUserServiceMock.Setup(x => x.UserId).Returns(userId.ToString());

            var workout = new Workout
            {
                Id = 1,
                Name = "Test Workout",
                WorkoutExercises = new List<WorkoutExercise>
                {
                    new WorkoutExercise { ExerciseId = 1, Order = 1, Sets = 3, Reps = "10", RestTimeInSeconds = 60 }
                }
            };
            await _dbContext.Workouts.AddAsync(workout);
            await _dbContext.SaveChangesAsync();

            var command = new StartWorkoutSessionCommand(1, new StartWorkoutSessionDto { PlannedDuration = 45, Difficulty = "Intermediate" });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.WorkoutId.Should().Be(1);
            result.Data.status.Should().Be("InProgress");

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
            _publishEndpointMock.Verify(x => x.Publish<IWorkoutSessionStarted>(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
