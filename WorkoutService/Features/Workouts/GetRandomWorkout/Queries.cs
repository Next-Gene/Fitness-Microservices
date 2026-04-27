using MediatR;

namespace WorkoutService.Features.Workouts.GetRandomWorkout
{
    public class GetRandomWorkoutQuery : IRequest<WorkoutSuggestionDto>
    {
        public string? Category { get; set; }
        public string? Difficulty { get; set; }
        public int? MaxDuration { get; set; }
        public bool? NoEquipment { get; set; }
    }

    public class WorkoutSuggestionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public int DurationInMinutes { get; set; }
        public int CaloriesBurn { get; set; }
        public List<ExerciseSummary> Exercises { get; set; } = new();
    }

    public class ExerciseSummary
    {
        public string Name { get; set; } = string.Empty;
        public int Sets { get; set; }
        public string Reps { get; set; } = string.Empty;
    }
}