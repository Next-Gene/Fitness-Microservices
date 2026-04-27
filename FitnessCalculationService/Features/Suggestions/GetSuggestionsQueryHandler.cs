using FitnessCalculationService.Data;
using FitnessCalculationService.Data.Enums;
using FitnessCalculationService.Features.Suggestions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using FitnessCalculationService.Data;

namespace FitnessCalculationService.Features.Suggestions
{
    public class GetSuggestionsQueryHandler : IRequestHandler<GetSuggestionsQuery, SuggestionResultDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public GetSuggestionsQueryHandler(ApplicationDbContext context, IConfiguration configuration, HttpClient httpClient)
        {
            _context = context;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<SuggestionResultDto> Handle(GetSuggestionsQuery request, CancellationToken cancellationToken)
        {
            var profile = await _context.WeightGoalActivity
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (profile == null)
                throw new Exception("User profile not found.");

            var fitnessStat = await _context.UserFitnessStat
                .FirstOrDefaultAsync(u => u.UserId == request.UserId, cancellationToken);

            if (fitnessStat == null)
                throw new Exception("Fitness stats not found. Please calculate fitness first.");

            double bmr = profile.Gender?.ToUpper() switch
            {
                "M" => 10 * profile.Weight + 6.25 * profile.Height - 5 * profile.Age + 5,
                "F" => 10 * profile.Weight + 6.25 * profile.Height - 5 * profile.Age - 161,
                _ => 10 * profile.Weight + 6.25 * profile.Height - 5 * profile.Age
            };

            double activityFactor = profile.ActivityLevel switch
            {
                ActivityLevel.Rookie => 1.2,
                ActivityLevel.Beginner => 1.375,
                ActivityLevel.Intermediate => 1.55,
                ActivityLevel.Advance => 1.725,
                ActivityLevel.TrueBeast => 1.9,
                _ => 1.2
            };

            double tdee = bmr * activityFactor;
            double calorieTarget = profile.Goal switch
            {
                Goal.LoseWeight => tdee - 500,
                Goal.GetFitter => tdee,
                Goal.GainWeight => tdee + 300,
                _ => tdee
            };

            var result = new SuggestionResultDto
            {
                UserId = request.UserId,
                Tdee = (decimal)tdee,
                CalorieTarget = (decimal)calorieTarget,
                Goal = profile.Goal.ToString(),
                Difficulty = profile.ActivityLevel.ToString()
            };

            var mealPlan = await GetMatchingMealPlanAsync((int)calorieTarget, profile.Goal.ToString());
            if (mealPlan != null)
            {
                result.MealPlan = mealPlan;
                result.Meals = await GetMealsForPlanAsync(mealPlan.Name);
            }

            var workouts = await GetMatchingWorkoutsAsync(profile.Goal.ToString(), profile.ActivityLevel.ToString());
            result.Workouts = workouts;

            return result;
        }

        private async Task<MealPlanDto> GetMatchingMealPlanAsync(int calorieTarget, string goal)
        {
            try
            {
                var baseUrl = _configuration["Services:NutritionService"] ?? "http://localhost:5001";
                var response = await _httpClient.GetAsync($"{baseUrl}/api/meals/recommendations?PageNumber=1&PageSize=50");

                if (!response.IsSuccessStatusCode)
                    return GetDefaultMealPlan(calorieTarget, goal);

                var content = await response.Content.ReadFromJsonAsync<GetMealRecommendationsResponse>();
                if (content?.Data == null || !content.Data.Any())
                    return GetDefaultMealPlan(calorieTarget, goal);

                var closestCal = content.Data
                    .Where(m => m.NutritionFacts?.Calories > 0)
                    .Select(m => m.NutritionFacts!.Calories)
                    .OrderBy(c => Math.Abs(c - calorieTarget))
                    .FirstOrDefault();

                var planName = closestCal <= 1200 ? "WL-1200" :
                              closestCal <= 1500 ? "WL-1500" :
                              closestCal <= 1800 ? "WL-1800" :
                              closestCal <= 2500 ? "FIT-2000" : "GW-2500";

                return new MealPlanDto
                {
                    Name = planName,
                    Description = $"{goal} Plan",
                    CalorieTarget = calorieTarget
                };
            }
            catch
            {
                return GetDefaultMealPlan(calorieTarget, goal);
            }
        }

        private MealPlanDto GetDefaultMealPlan(int calorieTarget, string goal)
        {
            var name = calorieTarget <= 1200 ? "WL-1200" :
                      calorieTarget <= 1500 ? "WL-1500" :
                      calorieTarget <= 1800 ? "WL-1800" :
                      calorieTarget <= 2500 ? "FIT-2000" : "GW-2500";

            return new MealPlanDto
            {
                Name = name,
                Description = $"{goal} Plan",
                CalorieTarget = calorieTarget
            };
        }

        private async Task<List<MealDto>> GetMealsForPlanAsync(string planName)
        {
            try
            {
                var baseUrl = _configuration["Services:NutritionService"] ?? "http://localhost:5001";
                var response = await _httpClient.GetAsync($"{baseUrl}/api/meals/recommendations?PageNumber=1&PageSize=4");

                if (!response.IsSuccessStatusCode)
                    return new List<MealDto>();

                var content = await response.Content.ReadFromJsonAsync<GetMealRecommendationsResponse>();
                return content?.Data?.Take(4).Select(m => new MealDto
                {
                    Name = m.Name,
                    Description = m.Description,
                    MealType = m.mealType.ToString(),
                    NutritionFacts = new NutritionFactDto
                    {
                        Calories = m.NutritionFacts?.Calories ?? 0,
                        Protein = m.NutritionFacts?.Protein ?? 0,
                        Carbs = m.NutritionFacts?.Carbs ?? 0,
                        Fats = m.NutritionFacts?.Fats ?? 0
                    }
                }).ToList() ?? new List<MealDto>();
            }
            catch
            {
                return new List<MealDto>();
            }
        }

        private async Task<List<WorkoutDto>> GetMatchingWorkoutsAsync(string goal, string difficulty)
        {
            try
            {
                var baseUrl = _configuration["Services:WorkoutService"] ?? "http://localhost:5002";
                var category = goal == "LoseWeight" ? "full-body" : goal == "GainWeight" ? "strength" : "full-body";
                var response = await _httpClient.GetAsync($"{baseUrl}/api/workouts?Category={category}&Difficulty={difficulty}");

                if (!response.IsSuccessStatusCode)
                    return GetDefaultWorkouts(goal, difficulty);

                var content = await response.Content.ReadFromJsonAsync<GetWorkoutsResponse>();
                return content?.Data?.Take(3).Select(w => new WorkoutDto
                {
                    Name = w.Name,
                    Description = w.Description,
                    Difficulty = w.Difficulty,
                    DurationInMinutes = w.DurationInMinutes,
                    CaloriesBurn = w.CaloriesBurn
                }).ToList() ?? GetDefaultWorkouts(goal, difficulty);
            }
            catch
            {
                return GetDefaultWorkouts(goal, difficulty);
            }
        }

        private List<WorkoutDto> GetDefaultWorkouts(string goal, string difficulty)
        {
            return new List<WorkoutDto>
            {
                new() { Name = "Full Body Introduction", Description = "A workout to learn the basic movements.", Difficulty = "Beginner", DurationInMinutes = 20, CaloriesBurn = 150 },
                new() { Name = "Beginner Bodyweight Circuit", Description = "A simple circuit to get started.", Difficulty = "Beginner", DurationInMinutes = 25, CaloriesBurn = 200 },
                new() { Name = "Active Recovery Day", Description = "Light workout for recovery.", Difficulty = "Beginner", DurationInMinutes = 20, CaloriesBurn = 100 }
            };
        }
    }

    public class GetMealRecommendationsResponse
    {
        public List<GetMealRecommendationItem> Data { get; set; } = new();
        public int TotalCount { get; set; }
    }

    public class GetMealRecommendationItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string mealType { get; set; } = default!;
        public NutritionFactItem? NutritionFacts { get; set; }
    }

    public class NutritionFactItem
    {
        public int Calories { get; set; }
        public double Protein { get; set; }
        public double Carbs { get; set; }
        public double Fats { get; set; }
    }

    public class GetWorkoutsResponse
    {
        public List<WorkoutItem> Data { get; set; } = new();
    }

    public class WorkoutItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Difficulty { get; set; } = default!;
        public int DurationInMinutes { get; set; }
        public int CaloriesBurn { get; set; }
    }
}