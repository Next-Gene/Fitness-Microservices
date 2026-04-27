using MediatR;

namespace NutritionService.Features.Suggestions
{
    public class GetRandomMealSuggestionQuery : IRequest<RandomMealSuggestionDto>
    {
        public string? MealType { get; set; }
        public int? MaxCalories { get; set; }
        public string? Goal { get; set; }
    }

    public class RandomMealSuggestionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string MealType { get; set; } = default!;
        public string Difficulty { get; set; } = default!;
        public int PrepTimeInMinutes { get; set; }
        public NutritionSummaryDto NutritionFacts { get; set; } = new();
        public List<string> MainIngredients { get; set; } = new();
    }

    public class NutritionSummaryDto
    {
        public int Calories { get; set; }
        public double Protein { get; set; }
        public double Carbs { get; set; }
        public double Fats { get; set; }
    }
}