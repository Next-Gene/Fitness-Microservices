using NutritionService.Domain.Models;
using NutritionService.Domain.Models.Enums;

namespace NutritionService.Features.Meals.Filters
{
    public static class MealFilter
    {
        public static IQueryable<Meal> ApplyFilters(
            IQueryable<Meal> query,
            string? mealType,
            int? maxCalories,
            int? minProtein)
        {
            if (!string.IsNullOrWhiteSpace(mealType) && Enum.TryParse<MealType>(mealType, true, out var typeEnum))
                query = query.Where(m => m.mealType == typeEnum);

            if (maxCalories.HasValue)
                query = query.Where(m => m.NutritionFacts.Calories <= maxCalories);

            if (minProtein.HasValue)
                query = query.Where(m => m.NutritionFacts.Protein >= minProtein);

            return query;
        }
    }
}
