using System.Text.Json.Serialization;

namespace ProgressTrackingService.Features.LogWeight
{
    public record LogWeightRequest(
        [property: JsonPropertyName("userId")] Guid UserId,
        [property: JsonPropertyName("weightKg")] decimal WeightKg,
        [property: JsonPropertyName("loggedAt")] DateTimeOffset LoggedAt,
        [property: JsonPropertyName("clientRequestId")] string ClientRequestId
    );
}
