using MediatR;
using WorkoutService.Features.Shared;
using WorkoutService.Features.Workouts.StartWorkoutSession.Dtos;
using WorkoutService.Features.Workouts.StartWorkoutSession.ViewModels;

namespace WorkoutService.Features.Workouts.StartWorkoutSession
{
    public record StartWorkoutSessionCommand(int WorkoutId, Guid UserId, StartWorkoutSessionDto Dto) : IRequest<RequestResponse<WorkoutSessionViewModel>>;
}
