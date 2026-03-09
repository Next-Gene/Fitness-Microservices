using MediatR;

namespace AuthenticationService.Features.Auth.UpdateUserProfile
{
    using MediatR;
    using Microsoft.AspNetCore.Http;

    namespace AuthenticationService.Features.Auth.UpdateUserProfile
    {
        public record UpdateUserProfileOrchestratorRequest(
            Guid UserId,
            string? FirstName,
            string? LastName,
            string? PhoneNumber,
            string Goal,
            string activtyLevel,
            double Weight,
            double Height,
            IFormFile? ProfileImage
        ) : IRequest<UpdateUserProfileResponse>;
    }
}
