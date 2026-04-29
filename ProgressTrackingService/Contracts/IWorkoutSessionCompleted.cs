namespace WorkoutService.Contracts
{
    public interface IWorkoutSessionCompleted
    {
        string SessionId { get; }
        Guid WorkoutId { get; }
        Guid UserId { get; }
        int DurationMinutes { get; }
        int TotalCaloriesBurned { get; }
        DateTime CompletedAt { get; }
    }
}
