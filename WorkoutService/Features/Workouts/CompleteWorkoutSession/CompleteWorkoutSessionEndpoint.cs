using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutService.Features.Shared;

namespace WorkoutService.Features.Workouts.CompleteWorkoutSession
{
    public static class Endpoints
    {
        public static void MapCompleteWorkoutSessionEndpoint(this WebApplication app)
        {
            app.MapPost("/api/v1/workouts/session/{sessionId}/complete", async (
                [FromServices] IMediator mediator,
                [FromRoute] int sessionId,
                [FromBody] CompleteSessionRequest request) =>
            {
                var command = new CompleteWorkoutSessionCommand
                {
                    SessionId = sessionId,
                    DurationMinutes = request.DurationMinutes,
                    CaloriesBurned = request.CaloriesBurned
                };

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

                return Results.Ok(new EndpointResponse<string>(
                    result.Data,
                    result.Message,
                    true,
                    200,
                    null,
                    DateTime.UtcNow
                ));
            })
            .RequireAuthorization();
        }
    }

    public class CompleteSessionRequest
    {
        public int DurationMinutes { get; set; }
        public int CaloriesBurned { get; set; }
    }
}
