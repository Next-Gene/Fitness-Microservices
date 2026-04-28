using System.Text.Json.Serialization;

namespace WorkoutService.Features.Workouts.StartWorkoutSession.Dtos
{
    public class StartWorkoutSessionDto
    {
        [JsonPropertyName("difficulty")]
        public string Difficulty { get; set; }

        [JsonPropertyName("plannedDuration")]
        public int PlannedDuration { get; set; }
    }
}
