using MediatR;
using NutritionService.Features.Suggestions;

namespace NutritionService.Features.Suggestions
{
    public static class MealSuggestionEndpoint
    {
        public static void MapMealSuggestionEndpoint(this WebApplication app)
        {
            app.MapGet("/api/v1/nutrition/random-meal",
                async ([AsParameters] GetRandomMealSuggestionQuery query, IMediator mediator) =>
                {
                    var result = await mediator.Send(query);
                    return Results.Ok(result);
                });

            app.MapGet("/api/v1/nutrition/daily-tip",
                async ([AsParameters] GetDailyTipQuery query, IMediator mediator) =>
                {
                    var result = await mediator.Send(query);
                    return Results.Ok(result);
                });
        }
    }
}