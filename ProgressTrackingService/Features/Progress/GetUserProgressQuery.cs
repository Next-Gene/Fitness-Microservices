using MediatR;
using ProgressTrackingService.Shared;
using System.Text.Json.Serialization;

namespace ProgressTrackingService.Features.Progress
{
    public record GetUserProgressQuery(
        [property: JsonPropertyName("userId")] Guid UserId,
        [property: JsonPropertyName("period")] string Period = "month"
    ) : IRequest<EndpointResponse<ProgressDashboardDto>>;

}
