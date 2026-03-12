using MediatR;
using AuthenticationService.Contarcts;
using AuthenticationService.Features.Auth.UpdateUserProfile.AuthenticationService.Features.Auth.UpdateUserProfile;

namespace AuthenticationService.Features.Auth.UpdateUserProfile
{
    public class UpdateUserProfileOrchestrator
        : IRequestHandler<UpdateUserProfileOrchestratorRequest, UpdateUserProfileResponse>
    {
        private readonly IMediator _mediator;
        private readonly IImageHelper _imageHelper;

        public UpdateUserProfileOrchestrator(
            IMediator mediator,
            IImageHelper imageHelper)
        {
            _mediator = mediator;
            _imageHelper = imageHelper;
        }

        public async Task<UpdateUserProfileResponse> Handle(
            UpdateUserProfileOrchestratorRequest request,
            CancellationToken cancellationToken)
        {
            string? imagePath = null;

            // 1️⃣ Upload Image
            if (request.ProfileImage is not null)
            {
                imagePath = await _imageHelper.SaveImageAsync(
                    request.ProfileImage,
                    "Users"
                );
            }

            // 2️⃣ Create Command
            var command = new UpdateUserProfileCommand(
                request.UserId,
                request.FirstName,
                request.LastName,
                request.PhoneNumber,
                imagePath,
                request.Goal,
                request.activtyLevel,
                request.Height,
                request.Weight
            );

            // 3️⃣ Send Command to Handler
            var result = await _mediator.Send(command, cancellationToken);

            // 4️⃣ Convert image path to full URL
            if (!string.IsNullOrEmpty(result.ProfileImageUrl))
            {
                result = result with
                {
                    ProfileImageUrl = _imageHelper.GetImageUrl(result.ProfileImageUrl)
                };
            }

            return result;
        }
    }
}