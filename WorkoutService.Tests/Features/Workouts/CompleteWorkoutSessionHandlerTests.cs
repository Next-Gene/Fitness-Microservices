using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Moq;
using WorkoutService.Contracts;
using WorkoutService.Domain.Entities;
using WorkoutService.Domain.Interfaces;
using WorkoutService.Features.Workouts.CompleteWorkoutSession;
using WorkoutService.Infrastructure;
using WorkoutService.Infrastructure.Data;
using Xunit;

namespace WorkoutService.Tests.Features.Workouts
{
    public class CompleteWorkoutSessionHandlerTests
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly BaseRepository<WorkoutSession> _sessionRepository;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IPublishEndpoint> _publishEndpointMock;
        private readonly CompleteWorkoutSessionHandler _handler;

        public CompleteWorkoutSessionHandlerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _sessionRepository = new BaseRepository<WorkoutSession>(_dbContext);

            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _publishEndpointMock = new Mock<IPublishEndpoint>();

            _handler = new CompleteWorkoutSessionHandler(
                _sessionRepository,
                _currentUserServiceMock.Object,
                _unitOfWorkMock.Object,
                _publishEndpointMock.Object
            );
        }

        [Fact]
        public async Task Handle_UserNotAuthenticated_ReturnsFail()
        {
            // Arrange
            _currentUserServiceMock.Setup(x => x.IsAuthenticated).Returns(false);
            var command = new CompleteWorkoutSessionCommand { SessionId = 1, DurationMinutes = 45, CaloriesBurned = 300 };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("User is not authenticated");
        }

        [Fact]
        public async Task Handle_SessionNotFound_ReturnsFail()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _currentUserServiceMock.Setup(x => x.IsAuthenticated).Returns(true);
            _currentUserServiceMock.Setup(x => x.UserId).Returns(userId.ToString());

            var command = new CompleteWorkoutSessionCommand { SessionId = 999, DurationMinutes = 45, CaloriesBurned = 300 };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Session not found or unauthorized.");
        }

        [Fact]
        public async Task Handle_ValidRequest_CompletesSessionAndPublishesEvent()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _currentUserServiceMock.Setup(x => x.IsAuthenticated).Returns(true);
            _currentUserServiceMock.Setup(x => x.UserId).Returns(userId.ToString());

            var session = new WorkoutSession
            {
                Id = 1,
                UserId = userId,
                WorkoutId = 1,
                Status = "InProgress",
                Difficulty = "Beginner",
                StartedAt = DateTime.UtcNow.AddMinutes(-45)
            };
            await _dbContext.WorkoutSessions.AddAsync(session);
            await _dbContext.SaveChangesAsync();

            var command = new CompleteWorkoutSessionCommand { SessionId = 1, DurationMinutes = 45, CaloriesBurned = 300 };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().Be("1");

            var updatedSession = await _dbContext.WorkoutSessions.FindAsync(1);
            updatedSession.Should().NotBeNull();
            updatedSession!.Status.Should().Be("Completed");
            updatedSession.EndedAt.Should().NotBeNull();

            _publishEndpointMock.Verify(x => x.Publish<IWorkoutSessionCompleted>(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
