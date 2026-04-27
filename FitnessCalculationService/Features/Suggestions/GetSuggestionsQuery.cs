using MediatR;

namespace FitnessCalculationService.Features.Suggestions
{
    public class GetSuggestionsQuery : IRequest<SuggestionResultDto>
    {
        public Guid UserId { get; set; }
    }
}