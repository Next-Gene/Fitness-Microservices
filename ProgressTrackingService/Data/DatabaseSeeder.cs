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

            // Seed some default users (matches IDs from IdentitySeeder if possible, or just Guid.Empty for now)
            var userId1 = Guid.Parse("A1B2C3D4-E5F6-4A1B-8C2D-3E4F5A6B7C8D"); // Example User ID
            var userId2 = Guid.Parse("B2C3D4E5-F6A1-4B2C-9D3E-4F5A6B7C8D9E"); // Another example

            var weightEntries = new List<WeightEntry>
            {
                new() { UserId = userId1, WeightKg = 90, LogDate = DateTime.UtcNow.AddMonths(-3) },
                new() { UserId = userId1, WeightKg = 88, LogDate = DateTime.UtcNow.AddMonths(-2) },
                new() { UserId = userId1, WeightKg = 86, LogDate = DateTime.UtcNow.AddMonths(-1) },
                new() { UserId = userId1, WeightKg = 85, LogDate = DateTime.UtcNow },

                new() { UserId = userId2, WeightKg = 70, LogDate = DateTime.UtcNow.AddMonths(-2) },
                new() { UserId = userId2, WeightKg = 71, LogDate = DateTime.UtcNow.AddMonths(-1) },
                new() { UserId = userId2, WeightKg = 73, LogDate = DateTime.UtcNow }
            };

            var workoutLogs = new List<WorkoutLog>
            {
                new() { UserId = userId1, WorkoutId = Guid.NewGuid(), WorkoutName = "Morning Cardio", DurationMinutes = 45, CaloriesBurned = 400, LogDate = DateTime.UtcNow.AddDays(-5) },
                new() { UserId = userId1, WorkoutId = Guid.NewGuid(), WorkoutName = "Full Body Strength", DurationMinutes = 60, CaloriesBurned = 350, LogDate = DateTime.UtcNow.AddDays(-3) },
                new() { UserId = userId1, WorkoutId = Guid.NewGuid(), WorkoutName = "Evening Yoga", DurationMinutes = 30, CaloriesBurned = 150, LogDate = DateTime.UtcNow.AddDays(-1) },

                new() { UserId = userId2, WorkoutId = Guid.NewGuid(), WorkoutName = "Leg Day", DurationMinutes = 75, CaloriesBurned = 500, LogDate = DateTime.UtcNow.AddDays(-4) },
                new() { UserId = userId2, WorkoutId = Guid.NewGuid(), WorkoutName = "Upper Body", DurationMinutes = 60, CaloriesBurned = 300, LogDate = DateTime.UtcNow.AddDays(-2) }
            };

            var statistics = new List<UserStatistics>
            {
                new() { UserId = userId1, TotalWorkouts = 3, TotalWeightLost = 5, CurrentWeight = 85, StartingWeight = 90, LastWorkoutDate = DateTime.UtcNow.AddDays(-1) },
                new() { UserId = userId2, TotalWorkouts = 2, TotalWeightLost = -3, CurrentWeight = 73, StartingWeight = 70, LastWorkoutDate = DateTime.UtcNow.AddDays(-2) }
            };

            await ctx.WeightEntries.AddRangeAsync(weightEntries);
            await ctx.WorkoutLogs.AddRangeAsync(workoutLogs);
            await ctx.UserStatistics.AddRangeAsync(statistics);
            await ctx.SaveChangesAsync();
        }
    }
}
