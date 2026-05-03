using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MassTransit;
using Moq;
using WorkoutService.Contracts;
using WorkoutService.Domain.Interfaces;
using WorkoutService.Features.Workouts.CreateWorkout;
using WorkoutService.Features.Workouts.CreateWorkout.ViewModels;
using Xunit;

namespace WorkoutService.Tests.Features.Workouts
{
    public class CreateWorkoutHandlerTests
    {
        private readonly Mock<IPublishEndpoint> _publishEndpointMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly CreateWorkoutHandler _handler;

        public CreateWorkoutHandlerTests()
        {
            _publishEndpointMock = new Mock<IPublishEndpoint>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new CreateWorkoutHandler(_publishEndpointMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldPublishEventAndSaveChanges()
        {
            // Arrange
            var dto = new CreateWorkoutDto(
                Name: "Test Workout",
                Description: "Test Description",
                CaloriesBurn: 500,
                IsPremium: false,
                Rating: 0.0,
                DurationInMinutes: 60,
                Difficulty: "Beginner",
                Category: "Cardio",
                workoutPlanId: 1
            );
            var command = new CreateWorkoutCommand(dto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(dto.Name);
            result.Description.Should().Be(dto.Description);

            // Verify publish was called
            _publishEndpointMock.Verify(
                x => x.Publish<IWorkoutCreated>(It.IsAny<object>(), It.IsAny<CancellationToken>()),
                Times.Once);

            // Verify SaveChangesAsync was called
            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
