using System.Text.Json.Serialization;

namespace WorkoutService.Features.Workouts.StartWorkoutSession.Dtos
{
    public class StartWorkoutSessionDto
    {
        [property: JsonPropertyName("difficulty")]
        public string Difficulty { get; set; }

        [property: JsonPropertyName("plannedDuration")]
        public int PlannedDuration { get; set; }
    }
}
