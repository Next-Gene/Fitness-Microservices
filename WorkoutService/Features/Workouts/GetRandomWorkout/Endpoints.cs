using MediatR;
using WorkoutService.Features.Workouts.GetRandomWorkout;

namespace WorkoutService.Features.Workouts.GetRandomWorkout
{
    public static class GetRandomWorkoutEndpoint
    {
        public static void MapGetRandomWorkoutEndpoint(this WebApplication app)
        {
            app.MapGet("/api/v1/workouts/random",
                async ([AsParameters] GetRandomWorkoutQuery query, IMediator mediator) =>
                {
                    var result = await mediator.Send(query);
                    return Results.Ok(result);
                });
        }
    }
}