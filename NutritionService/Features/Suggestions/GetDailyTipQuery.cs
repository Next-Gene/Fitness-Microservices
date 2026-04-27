using MediatR;

namespace NutritionService.Features.Suggestions
{
    public class GetDailyTipQuery : IRequest<DailyTipDto>
    {
        public string? Category { get; set; }
    }

    public class DailyTipDto
    {
        public string Tip { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
    }
}