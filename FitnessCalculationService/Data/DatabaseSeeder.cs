using Microsoft.EntityFrameworkCore;
using FitnessCalculationService.Data;

namespace FitnessCalculationService.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await SeedWorkoutPlansAsync(ctx);
            await SeedFitnessPlanConfigsAsync(ctx);
        }

        private static async Task SeedWorkoutPlansAsync(ApplicationDbContext ctx)
        {
            if (await ctx.WorkoutPlans.AnyAsync()) return;

            var plans = new List<WorkoutPlandb>
            {
                new() { PlanId = Guid.NewGuid(), PlanName = "Weight Loss - Beginner", Description = "Low intensity workouts for beginners", DurationWeeks = 4, Difficulty = "Easy" },
                new() { PlanId = Guid.NewGuid(), PlanName = "Weight Loss - Intermediate", Description = "Medium intensity fat burning", DurationWeeks = 8, Difficulty = "Normal" },
                new() { PlanId = Guid.NewGuid(), PlanName = "Weight Loss - Advanced", Description = "High intensity metabolic conditioning", DurationWeeks = 12, Difficulty = "Hard" },
                new() { PlanId = Guid.NewGuid(), PlanName = "Muscle Gain - Beginner", Description = "Fundamental strength training", DurationWeeks = 6, Difficulty = "Easy" },
                new() { PlanId = Guid.NewGuid(), PlanName = "Muscle Gain - Intermediate", Description = "Hypertrophy focused training", DurationWeeks = 10, Difficulty = "Normal" },
                new() { PlanId = Guid.NewGuid(), PlanName = "Muscle Gain - Advanced", Description = "Powerbuilding and advanced techniques", DurationWeeks = 12, Difficulty = "Hard" },
                new() { PlanId = Guid.NewGuid(), PlanName = "General Fitness - Normal", Description = "Balanced maintenance routine", DurationWeeks = 8, Difficulty = "Normal" }
            };

            await ctx.WorkoutPlans.AddRangeAsync(plans);
            await ctx.SaveChangesAsync();
        }

        private static async Task SeedFitnessPlanConfigsAsync(ApplicationDbContext ctx)
        {
            if (await ctx.FitnessPlanConfig.AnyAsync()) return;

            var plans = await ctx.WorkoutPlans.ToListAsync();
            
            var configs = new List<FitnessPlanConfigdb>();

            // Lose Weight
            var wlBeginner = plans.First(p => p.PlanName.Contains("Weight Loss") && p.Difficulty == "Easy");
            var wlNormal = plans.First(p => p.PlanName.Contains("Weight Loss") && p.Difficulty == "Normal");
            var wlHard = plans.First(p => p.PlanName.Contains("Weight Loss") && p.Difficulty == "Hard");

            configs.Add(new() { Goal = "Lose Weight", Status = "Easy", MinCalorie = 1200, MaxCalorie = 1500, PlanType = "Nutrition", WorkoutPlanId = 0, WorkoutPlan = wlBeginner });
            configs.Add(new() { Goal = "Lose Weight", Status = "Normal", MinCalorie = 1500, MaxCalorie = 2000, PlanType = "Combined", WorkoutPlanId = 0, WorkoutPlan = wlNormal });
            configs.Add(new() { Goal = "Lose Weight", Status = "Hard", MinCalorie = 2000, MaxCalorie = 2500, PlanType = "Combined", WorkoutPlanId = 0, WorkoutPlan = wlHard });

            // Gain Weight
            var mgBeginner = plans.First(p => p.PlanName.Contains("Muscle Gain") && p.Difficulty == "Easy");
            var mgNormal = plans.First(p => p.PlanName.Contains("Muscle Gain") && p.Difficulty == "Normal");
            var mgHard = plans.First(p => p.PlanName.Contains("Muscle Gain") && p.Difficulty == "Hard");

            configs.Add(new() { Goal = "Gain Weight", Status = "Easy", MinCalorie = 2500, MaxCalorie = 2800, PlanType = "Nutrition", WorkoutPlanId = 0, WorkoutPlan = mgBeginner });
            configs.Add(new() { Goal = "Gain Weight", Status = "Normal", MinCalorie = 2800, MaxCalorie = 3200, PlanType = "Combined", WorkoutPlanId = 0, WorkoutPlan = mgNormal });
            configs.Add(new() { Goal = "Gain Weight", Status = "Hard", MinCalorie = 3200, MaxCalorie = 4000, PlanType = "Combined", WorkoutPlanId = 0, WorkoutPlan = mgHard });

            // Get Fitter / Maintenance
            var maintenance = plans.First(p => p.PlanName.Contains("General Fitness"));
            configs.Add(new() { Goal = "Get Fitter", Status = "Normal", MinCalorie = 2000, MaxCalorie = 2500, PlanType = "Combined", WorkoutPlanId = 0, WorkoutPlan = maintenance });

            await ctx.FitnessPlanConfig.AddRangeAsync(configs);
            await ctx.SaveChangesAsync();
        }
    }
}
