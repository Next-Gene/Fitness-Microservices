using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutService.Features.Shared;
using WorkoutService.Features.Workouts.StartWorkoutSession.Dtos;
using WorkoutService.Features.Workouts.StartWorkoutSession.ViewModels;

namespace WorkoutService.Features.Workouts.StartWorkoutSession
{
    public static class Endpoints
    {
        public static void MapStartWorkoutSessionEndpoint(this WebApplication app)
        {
            // Primary endpoint with v1
            app.MapPost("/api/v1/workouts/{id}/start", async (
                [FromServices] IMediator mediator,
                [FromRoute] int id,
                [FromBody] StartWorkoutSessionDto dto) =>
            {
                var result = await HandleStartWorkout(mediator, id, dto);
                return result;
            })
            .RequireAuthorization();

            // Fallback endpoint without v1 for frontend compatibility
            app.MapPost("/api/workouts/{id}/start", async (
                [FromServices] IMediator mediator,
                [FromRoute] int id,
                [FromBody] StartWorkoutSessionDto dto) =>
            {
                var result = await HandleStartWorkout(mediator, id, dto);
                return result;
            })
            .RequireAuthorization();
        }

        private static async Task<IResult> HandleStartWorkout(IMediator mediator, int id, StartWorkoutSessionDto dto)
        {
            var command = new StartWorkoutSessionCommand(id, dto);
            var result = await mediator.Send(command);

            if (!result.IsSuccess)
            {
                var statusCode = result.Message.Contains("not authenticated") ? 401 : 400;
                var response = new EndpointResponse<object>(
                    null,
                    result.Message,
                    false,
                    statusCode,
                    new List<string> { result.Message },
                    DateTime.UtcNow
                );

                return Results.Json(response, statusCode: statusCode);
            }

            return Results.Ok(new EndpointResponse<WorkoutSessionViewModel>(
                result.Data,
                result.Message,
                true,
                200,
                null,
                DateTime.UtcNow
            ));
        }
    }
}
