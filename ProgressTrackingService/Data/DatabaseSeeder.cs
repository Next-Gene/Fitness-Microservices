using Microsoft.EntityFrameworkCore;
using ProgressTrackingService.Models;
using ProgressTrackingService.Data;

namespace ProgressTrackingService.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<ProgressDbContext>();

            if (await ctx.WeightEntries.AnyAsync()) return;

            // Seed some default users (matches IDs from IdentitySeeder if possible)
            var userId1 = Guid.Parse("A1B2C3D4-E5F6-4A1B-8C2D-3E4F5A6B7C8D"); 
            var userId2 = Guid.Parse("B2C3D4E5-F6A1-4B2C-9D3E-4F5A6B7C8D9E"); 

            var weightEntries = new List<WeightEntry>
            {
                new() { UserId = userId1, WeightKg = 90, LoggedAt = DateTimeOffset.UtcNow.AddMonths(-3) },
                new() { UserId = userId1, WeightKg = 88, LoggedAt = DateTimeOffset.UtcNow.AddMonths(-2) },
                new() { UserId = userId1, WeightKg = 86, LoggedAt = DateTimeOffset.UtcNow.AddMonths(-1) },
                new() { UserId = userId1, WeightKg = 85, LoggedAt = DateTimeOffset.UtcNow },

                new() { UserId = userId2, WeightKg = 70, LoggedAt = DateTimeOffset.UtcNow.AddMonths(-2) },
                new() { UserId = userId2, WeightKg = 71, LoggedAt = DateTimeOffset.UtcNow.AddMonths(-1) },
                new() { UserId = userId2, WeightKg = 73, LoggedAt = DateTimeOffset.UtcNow }
            };

            var workoutLogs = new List<WorkoutLog>
            {
                new() { UserId = userId1, WorkoutId = Guid.NewGuid(), SessionId = Guid.NewGuid(), DurationMinutes = 45, CaloriesBurned = 400, PerformedAt = DateTimeOffset.UtcNow.AddDays(-5) },
                new() { UserId = userId1, WorkoutId = Guid.NewGuid(), SessionId = Guid.NewGuid(), DurationMinutes = 60, CaloriesBurned = 350, PerformedAt = DateTimeOffset.UtcNow.AddDays(-3) },
                new() { UserId = userId1, WorkoutId = Guid.NewGuid(), SessionId = Guid.NewGuid(), DurationMinutes = 30, CaloriesBurned = 150, PerformedAt = DateTimeOffset.UtcNow.AddDays(-1) },

                new() { UserId = userId2, WorkoutId = Guid.NewGuid(), SessionId = Guid.NewGuid(), DurationMinutes = 75, CaloriesBurned = 500, PerformedAt = DateTimeOffset.UtcNow.AddDays(-4) },
                new() { UserId = userId2, WorkoutId = Guid.NewGuid(), SessionId = Guid.NewGuid(), DurationMinutes = 60, CaloriesBurned = 300, PerformedAt = DateTimeOffset.UtcNow.AddDays(-2) }
            };

            var statistics = new List<UserStatistics>
            {
                new() { UserId = userId1, TotalWorkouts = 3, TotalCaloriesBurned = 900, CurrentWeight = 85, StartingWeight = 90, LastWorkoutAt = DateTimeOffset.UtcNow.AddDays(-1) },
                new() { UserId = userId2, TotalWorkouts = 2, TotalCaloriesBurned = 800, CurrentWeight = 73, StartingWeight = 70, LastWorkoutAt = DateTimeOffset.UtcNow.AddDays(-2) }
            };

            await ctx.WeightEntries.AddRangeAsync(weightEntries);
            await ctx.WorkoutLogs.AddRangeAsync(workoutLogs);
            await ctx.UserStatistics.AddRangeAsync(statistics);
            await ctx.SaveChangesAsync();
        }
    }
}
