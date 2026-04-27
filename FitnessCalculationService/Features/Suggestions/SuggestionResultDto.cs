namespace FitnessCalculationService.Features.Suggestions
{
    public class SuggestionResultDto
    {
        public Guid UserId { get; set; }
        public decimal Tdee { get; set; }
        public decimal CalorieTarget { get; set; }
        public string Goal { get; set; }
        public string Difficulty { get; set; }
        public MealPlanDto? MealPlan { get; set; }
        public List<MealDto> Meals { get; set; } = new();
        public List<WorkoutDto> Workouts { get; set; } = new();
    }

    public class MealPlanDto
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public int CalorieTarget { get; set; }
    }

    public class MealDto
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string MealType { get; set; } = default!;
        public NutritionFactDto NutritionFacts { get; set; } = new();
    }

    public class NutritionFactDto
    {
        public int Calories { get; set; }
        public double Protein { get; set; }
        public double Carbs { get; set; }
        public double Fats { get; set; }
    }

    public class WorkoutDto
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Difficulty { get; set; } = default!;
        public int DurationInMinutes { get; set; }
        public int CaloriesBurn { get; set; }
    }
}