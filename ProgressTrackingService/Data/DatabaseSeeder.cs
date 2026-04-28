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

            var adminUserId = Guid.Parse("11111111-2222-3333-4444-555555555555");

            // Clear existing data for this user to ensure we can re-seed with the new full data
            var existingWeights = await ctx.WeightEntries.Where(w => w.UserId == adminUserId).ToListAsync();
            if (existingWeights.Any()) ctx.WeightEntries.RemoveRange(existingWeights);
            
            var existingLogs = await ctx.WorkoutLogs.Where(w => w.UserId == adminUserId).ToListAsync();
            if (existingLogs.Any()) ctx.WorkoutLogs.RemoveRange(existingLogs);
            
            var existingStats = await ctx.UserStatistics.Where(s => s.UserId == adminUserId).ToListAsync();
            if (existingStats.Any()) ctx.UserStatistics.RemoveRange(existingStats);

            await ctx.SaveChangesAsync();

            var weightEntries = new List<WeightEntry>();
            var now = DateTimeOffset.UtcNow;
            
            // 3 months of weekly weight entries
            for (int i = 90; i >= 0; i -= 7)
            {
                weightEntries.Add(new WeightEntry { 
                    UserId = adminUserId, 
                    WeightKg = 120m - (decimal)(90 - i) * 0.16m, // Smooth decline from 120 to ~105
                    LoggedAt = now.AddDays(-i) 
                });
            }

            var workoutLogs = new List<WorkoutLog>();
            // 20 workout logs over the last 30 days
            var random = new Random();
            for (int i = 30; i >= 1; i--)
            {
                if (i % 2 == 0 || i % 3 == 0) // Roughly 4-5 workouts per week
                {
                    workoutLogs.Add(new WorkoutLog { 
                        UserId = adminUserId, 
                        WorkoutId = Guid.NewGuid(), 
                        SessionId = Guid.NewGuid(), 
                        DurationMinutes = random.Next(30, 65), 
                        CaloriesBurned = random.Next(250, 550), 
                        PerformedAt = now.AddDays(-i).AddHours(random.Next(8, 20)), 
                        Rating = random.Next(3, 6) 
                    });
                }
            }

            var statistics = new List<UserStatistics>
            {
                new() { 
                    UserId = adminUserId, 
                    TotalWorkouts = workoutLogs.Count, 
                    TotalCaloriesBurned = workoutLogs.Sum(l => l.CaloriesBurned), 
                    CurrentWeight = 105.6m, 
                    StartingWeight = 120m, 
                    LastWorkoutAt = now.AddDays(-1), 
                    CurrentStreak = 4, 
                    LongestStreak = 7 
                }
            };

            await ctx.WeightEntries.AddRangeAsync(weightEntries);
            await ctx.WorkoutLogs.AddRangeAsync(workoutLogs);
            await ctx.UserStatistics.AddRangeAsync(statistics);
            await ctx.SaveChangesAsync();
        }
    }
}
