using MediatR;
using Microsoft.AspNetCore.Identity;
using AuthenticationService.Contarcts;
using AuthenticationService.Models;

namespace AuthenticationService.Features.Auth.UpdateUserProfile
{
    public class UpdateUserProfileHandler
        : IRequestHandler<UpdateUserProfileCommand, UpdateUserProfileResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;

        public UpdateUserProfileHandler(
            UserManager<ApplicationUser> userManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<UpdateUserProfileResponse> Handle(
            UpdateUserProfileCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (user is null)
                throw new KeyNotFoundException("User not found");

            // Update basic info
            if (!string.IsNullOrWhiteSpace(request.FirstName))
                user.FirstName = request.FirstName;

            if (!string.IsNullOrWhiteSpace(request.LastName))
                user.LastName = request.LastName;

            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
                user.PhoneNumber = request.PhoneNumber;

            if (!string.IsNullOrWhiteSpace(request.ProfileImage))
                user.ProfileImageUrl = request.ProfileImage;

            // Update fitness data
            if (request.Goal is not null)
                user.Goal = request.Goal;

            if (request.activtyLevel is not null)
                user.ActivtyLevel = request.activtyLevel;

            if (request.Height > 0)
                user.Height = request.Height;

            if (request.Weight > 0)
                user.Weight = request.Weight;

            user.FullName = $"{user.FirstName} {user.LastName}";

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                var errors = string.Join("; ", updateResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to update profile: {errors}");
            }

            // Generate tokens
            var roles = await _userManager.GetRolesAsync(user);
            var tokens = await _tokenService.GenerateTokensAsync(user, false);

            return new UpdateUserProfileResponse
            {
                Success = true,
                Message = "Profile updated successfully",
                UserId = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                lastName = user.LastName,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Goal = user.Goal,
                Age = user.Age,
                Height = user.Height,
                Weight = user.Weight,
                ActivtyLevel = user.ActivtyLevel,
                ProfileImageUrl = user.ProfileImageUrl,
                Roles = roles,
                Token = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken
            };
        }
    }
}