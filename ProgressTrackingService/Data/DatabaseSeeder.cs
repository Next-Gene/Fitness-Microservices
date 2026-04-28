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

            var adminUserId = Guid.Parse("11111111-2222-3333-4444-555555555555");

            var weightEntries = new List<WeightEntry>
            {
                new() { UserId = adminUserId, WeightKg = 120, LoggedAt = DateTimeOffset.UtcNow.AddMonths(-3) },
                new() { UserId = adminUserId, WeightKg = 118, LoggedAt = DateTimeOffset.UtcNow.AddDays(-90) },
                new() { UserId = adminUserId, WeightKg = 115, LoggedAt = DateTimeOffset.UtcNow.AddDays(-60) },
                new() { UserId = adminUserId, WeightKg = 112, LoggedAt = DateTimeOffset.UtcNow.AddDays(-30) },
                new() { UserId = adminUserId, WeightKg = 110, LoggedAt = DateTimeOffset.UtcNow.AddDays(-15) },
                new() { UserId = adminUserId, WeightKg = 108, LoggedAt = DateTimeOffset.UtcNow.AddDays(-7) },
                new() { UserId = adminUserId, WeightKg = 106, LoggedAt = DateTimeOffset.UtcNow.AddDays(-3) },
                new() { UserId = adminUserId, WeightKg = 105, LoggedAt = DateTimeOffset.UtcNow }
            };

            var workoutLogs = new List<WorkoutLog>
            {
                new() { UserId = adminUserId, WorkoutId = Guid.NewGuid(), SessionId = Guid.NewGuid(), DurationMinutes = 30, CaloriesBurned = 250, PerformedAt = DateTimeOffset.UtcNow.AddDays(-15), Rating = 4 },
                new() { UserId = adminUserId, WorkoutId = Guid.NewGuid(), SessionId = Guid.NewGuid(), DurationMinutes = 45, CaloriesBurned = 350, PerformedAt = DateTimeOffset.UtcNow.AddDays(-13), Rating = 5 },
                new() { UserId = adminUserId, WorkoutId = Guid.NewGuid(), SessionId = Guid.NewGuid(), DurationMinutes = 40, CaloriesBurned = 300, PerformedAt = DateTimeOffset.UtcNow.AddDays(-10), Rating = 4 },
                new() { UserId = adminUserId, WorkoutId = Guid.NewGuid(), SessionId = Guid.NewGuid(), DurationMinutes = 50, CaloriesBurned = 400, PerformedAt = DateTimeOffset.UtcNow.AddDays(-8), Rating = 5 },
                new() { UserId = adminUserId, WorkoutId = Guid.NewGuid(), SessionId = Guid.NewGuid(), DurationMinutes = 35, CaloriesBurned = 280, PerformedAt = DateTimeOffset.UtcNow.AddDays(-6), Rating = 4 },
                new() { UserId = adminUserId, WorkoutId = Guid.NewGuid(), SessionId = Guid.NewGuid(), DurationMinutes = 60, CaloriesBurned = 450, PerformedAt = DateTimeOffset.UtcNow.AddDays(-4), Rating = 5 },
                new() { UserId = adminUserId, WorkoutId = Guid.NewGuid(), SessionId = Guid.NewGuid(), DurationMinutes = 45, CaloriesBurned = 350, PerformedAt = DateTimeOffset.UtcNow.AddDays(-2), Rating = 4 },
                new() { UserId = adminUserId, WorkoutId = Guid.NewGuid(), SessionId = Guid.NewGuid(), DurationMinutes = 55, CaloriesBurned = 420, PerformedAt = DateTimeOffset.UtcNow.AddDays(-1), Rating = 5 }
            };

            var statistics = new List<UserStatistics>
            {
                new() { UserId = adminUserId, TotalWorkouts = 8, TotalCaloriesBurned = 2800, CurrentWeight = 105, StartingWeight = 120, LastWorkoutAt = DateTimeOffset.UtcNow.AddDays(-1), CurrentStreak = 3, LongestStreak = 5 }
            };

            await ctx.WeightEntries.AddRangeAsync(weightEntries);
            await ctx.WorkoutLogs.AddRangeAsync(workoutLogs);
            await ctx.UserStatistics.AddRangeAsync(statistics);
            await ctx.SaveChangesAsync();
        }
    }
}
