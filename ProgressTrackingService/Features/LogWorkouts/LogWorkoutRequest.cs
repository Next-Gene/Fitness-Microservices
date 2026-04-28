using System.Text.Json.Serialization;

namespace ProgressTrackingService.Features.LogWorkouts
{
    public record LogWorkoutRequest(
        [property: JsonPropertyName("userId")] Guid UserId,
        [property: JsonPropertyName("sessionId")] Guid SessionId,
        [property: JsonPropertyName("workoutId")] Guid? WorkoutId,
        [property: JsonPropertyName("durationMinutes")] int DurationMinutes,
        [property: JsonPropertyName("caloriesBurned")] int CaloriesBurned,
        [property: JsonPropertyName("rating")] int Rating,
        [property: JsonPropertyName("performedAt")] DateTimeOffset PerformedAt,
        [property: JsonPropertyName("clientRequestId")] string ClientRequestId
    );
}
