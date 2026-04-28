using MediatR;
using WorkoutService.Features.Shared;

namespace WorkoutService.Features.Workouts.CompleteWorkoutSession
{
    public class CompleteWorkoutSessionCommand : IRequest<RequestResponse<string>>
    {
        public int SessionId { get; set; }
        public int DurationMinutes { get; set; }
        public int CaloriesBurned { get; set; }
    }
}
