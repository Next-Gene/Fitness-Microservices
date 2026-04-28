using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutService.Features.Shared;
using WorkoutService.Features.Workouts.StartWorkoutSession.ViewModels;

namespace WorkoutService.Features.Workouts.GetWorkoutSession
{
    public static class Endpoints
    {
        public static void MapGetWorkoutSessionEndpoint(this WebApplication app)
        {
            app.MapGet("/api/v1/workouts/session/{sessionId}", async (
                [FromServices] IMediator mediator,
                [FromRoute] int sessionId) =>
            {
                var query = new GetWorkoutSessionQuery(sessionId);
                var result = await mediator.Send(query);

                if (!result.IsSuccess)
                {
                    var statusCode = result.Message.Contains("not authenticated") ? 401 : 404;
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
            })
            .RequireAuthorization();
        }
    }
}
