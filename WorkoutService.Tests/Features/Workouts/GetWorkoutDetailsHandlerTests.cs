using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using WorkoutService.Domain.Entities;
using WorkoutService.Features.Workouts.GetWorkoutDetails;
using WorkoutService.Infrastructure;
using WorkoutService.Infrastructure.Data;
using Xunit;

namespace WorkoutService.Tests.Features.Workouts
{
    public class GetWorkoutDetailsHandlerTests
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly BaseRepository<Workout> _workoutRepository;
        private readonly GetWorkoutDetailsHandler _handler;

        public GetWorkoutDetailsHandlerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _workoutRepository = new BaseRepository<Workout>(_dbContext);

            _handler = new GetWorkoutDetailsHandler(_workoutRepository);
        }

        [Fact]
        public async Task Handle_WorkoutExists_ReturnsWorkoutDetails()
        {
            // Arrange
            var workoutId = 1;
            var workout = new Workout
            {
                Id = workoutId,
                Name = "Test Workout",
                Description = "Test Description",
                Category = "Cardio",
                Difficulty = "Beginner",
                DurationInMinutes = 30,
                CaloriesBurn = 200,
                IsPremium = false,
                Rating = 4.5,
                WorkoutExercises = new List<WorkoutExercise>
                {
                    new WorkoutExercise
                    {
                        Exercise = new Exercise { Name = "Push-ups", Description = "Test", Difficulty = "Beginner", TargetMuscles = new List<string> { "Chest" }, EquipmentNeeded = new List<string> { "None" } },
                        Sets = 3,
                        Reps = "10",
                        RestTimeInSeconds = 60,
                        Order = 1
                    }
                }
            };

            await _dbContext.Workouts.AddAsync(workout);
            await _dbContext.SaveChangesAsync();

            var query = new GetWorkoutDetailsQuery(workoutId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Id.Should().Be(workoutId);
            result.Data.Name.Should().Be("Test Workout");
            result.Data.Exercises.Should().HaveCount(1);
            result.Data.Exercises[0].Name.Should().Be("Push-ups");
            result.Data.Variations.Should().NotBeNull();
            result.Data.Tips.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Handle_WorkoutDoesNotExist_ReturnsFail()
        {
            // Arrange
            var query = new GetWorkoutDetailsQuery(999);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Workout not found");
        }
    }
}
