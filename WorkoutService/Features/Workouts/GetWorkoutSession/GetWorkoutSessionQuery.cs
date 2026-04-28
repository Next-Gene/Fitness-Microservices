using MediatR;
using WorkoutService.Features.Shared;
using WorkoutService.Features.Workouts.StartWorkoutSession.ViewModels;

namespace WorkoutService.Features.Workouts.GetWorkoutSession
{
    public record GetWorkoutSessionQuery(int SessionId) : IRequest<RequestResponse<WorkoutSessionViewModel>>;
}
