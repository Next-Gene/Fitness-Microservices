using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WorkoutService.Domain.Entities;
using WorkoutService.Features.Workouts.GetAllWorkouts;
using WorkoutService.Infrastructure;
using WorkoutService.Infrastructure.Data;
using Xunit;

namespace WorkoutService.Tests.Features.Workouts
{
    public class GetAllWorkoutsHandlerTests
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly BaseRepository<Workout> _workoutRepository;
        private readonly IMemoryCache _cache;
        private readonly GetAllWorkoutsHandler _handler;

        public GetAllWorkoutsHandlerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _workoutRepository = new BaseRepository<Workout>(_dbContext);
            _cache = new MemoryCache(new MemoryCacheOptions());

            _handler = new GetAllWorkoutsHandler(_workoutRepository, _cache);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsPaginatedWorkouts()
        {
            // Arrange
            var workouts = new List<Workout>
            {
                new Workout { Id = 1, Name = "Cardio Blast", Category = "Cardio", Difficulty = "Beginner", DurationInMinutes = 30, Description = "Test", WorkoutExercises = new List<WorkoutExercise>() },
                new Workout { Id = 2, Name = "Strength Training", Category = "Strength", Difficulty = "Intermediate", DurationInMinutes = 45, Description = "Test", WorkoutExercises = new List<WorkoutExercise>() },
                new Workout { Id = 3, Name = "Yoga Core", Category = "Yoga", Difficulty = "Beginner", DurationInMinutes = 30, Description = "Test", WorkoutExercises = new List<WorkoutExercise>() }
            };

            await _dbContext.Workouts.AddRangeAsync(workouts);
            await _dbContext.SaveChangesAsync();

            var query = new GetAllWorkoutsQuery
            {
                Page = 1,
                PageSize = 10,
                Category = "Cardio"
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Items.Should().HaveCount(1);
            result.Data.Items[0].Name.Should().Be("Cardio Blast");
            result.Data.TotalCount.Should().Be(1);
        }

        [Fact]
        public async Task Handle_SearchAndDifficulty_FiltersCorrectly()
        {
            // Arrange
            var workouts = new List<Workout>
            {
                new Workout { Id = 4, Name = "Extreme HIIT", Category = "HIIT", Difficulty = "Advanced", DurationInMinutes = 20, Description = "Intense workout", WorkoutExercises = new List<WorkoutExercise>() },
                new Workout { Id = 5, Name = "Light Stretch", Category = "Yoga", Difficulty = "Beginner", DurationInMinutes = 15, Description = "Relaxing", WorkoutExercises = new List<WorkoutExercise>() }
            };

            await _dbContext.Workouts.AddRangeAsync(workouts);
            await _dbContext.SaveChangesAsync();

            var query = new GetAllWorkoutsQuery
            {
                Page = 1,
                PageSize = 10,
                Search = "HIIT",
                Difficulty = "Advanced"
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Items.Should().HaveCount(1);
            result.Data.Items[0].Name.Should().Be("Extreme HIIT");
        }
    }
}
