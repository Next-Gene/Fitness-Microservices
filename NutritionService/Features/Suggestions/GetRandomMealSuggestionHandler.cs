using MediatR;
using Microsoft.EntityFrameworkCore;
using NutritionService.Domain.Models.Enums;
using NutritionService.Infrastructure.Data;

namespace NutritionService.Features.Suggestions
{
    public class GetRandomMealSuggestionHandler : IRequestHandler<GetRandomMealSuggestionQuery, RandomMealSuggestionDto>
    {
        private readonly ApplicationDbContext _context;

        public GetRandomMealSuggestionHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RandomMealSuggestionDto> Handle(GetRandomMealSuggestionQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Meals
                .Include(m => m.NutritionFacts)
                .Include(m => m.MealIngredients)
                .ThenInclude(mi => mi.Ingredient)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.MealType))
            {
                if (Enum.TryParse<MealType>(request.MealType, true, out var mealType))
                    query = query.Where(m => m.mealType == mealType);
            }

            if (request.MaxCalories.HasValue)
            {
                query = query.Where(m => m.NutritionFacts != null && m.NutritionFacts.Calories <= request.MaxCalories.Value);
            }

            if (!string.IsNullOrEmpty(request.Goal))
            {
                query = request.Goal.ToLower() switch
                {
                    "loseweight" or "lose weight" => query.Where(m => m.NutritionFacts != null && m.NutritionFacts.Calories < 600),
                    "gainweight" or "gain weight" => query.Where(m => m.NutritionFacts != null && m.NutritionFacts.Calories >= 600),
                    _ => query
                };
            }

            var count = await query.CountAsync(cancellationToken);
            if (count == 0)
                throw new Exception("No meals found matching criteria.");

            var random = new Random();
            var randomSkip = random.Next(count);
            
            var meal = await query
                .Skip(randomSkip)
                .Take(1)
                .FirstOrDefaultAsync(cancellationToken);

            if (meal == null)
                meal = await query.FirstOrDefaultAsync(cancellationToken);

            return new RandomMealSuggestionDto
            {
                Id = meal.Id,
                Name = meal.Name,
                Description = meal.Description,
                MealType = meal.mealType.ToString(),
                Difficulty = meal.Difficulty,
                PrepTimeInMinutes = meal.PrepTimeInMinutes,
                NutritionFacts = new NutritionSummaryDto
                {
                    Calories = meal.NutritionFacts?.Calories ?? 0,
                    Protein = meal.NutritionFacts?.Protein ?? 0,
                    Carbs = meal.NutritionFacts?.Carbs ?? 0,
                    Fats = meal.NutritionFacts?.Fats ?? 0
                },
                MainIngredients = meal.MealIngredients?
                    .Take(5)
                    .Select(mi => mi.Ingredient?.Name ?? "")
                    .Where(n => !string.IsNullOrEmpty(n))
                    .ToList() ?? new List<string>()
            };
        }
    }
}