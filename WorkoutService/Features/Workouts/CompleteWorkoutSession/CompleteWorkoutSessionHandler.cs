using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkoutService.Contracts;
using WorkoutService.Domain.Entities;
using WorkoutService.Domain.Interfaces;
using WorkoutService.Features.Shared;

namespace WorkoutService.Features.Workouts.CompleteWorkoutSession
{
    public class CompleteWorkoutSessionHandler : IRequestHandler<CompleteWorkoutSessionCommand, RequestResponse<string>>
    {
        private readonly IBaseRepository<WorkoutSession> _sessionRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublishEndpoint _publishEndpoint;

        public CompleteWorkoutSessionHandler(
            IBaseRepository<WorkoutSession> sessionRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork,
            IPublishEndpoint publishEndpoint)
        {
            _sessionRepository = sessionRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<RequestResponse<string>> Handle(CompleteWorkoutSessionCommand request, CancellationToken cancellationToken)
        {
            if (!_currentUserService.IsAuthenticated || string.IsNullOrEmpty(_currentUserService.UserId))
            {
                return RequestResponse<string>.Fail("User is not authenticated");
            }

            if (!Guid.TryParse(_currentUserService.UserId, out Guid userIdGuid))
            {
                return RequestResponse<string>.Fail("Invalid user token format.");
            }

            var session = await _sessionRepository.GetAll()
                .FirstOrDefaultAsync(s => s.Id == request.SessionId && s.UserId == userIdGuid, cancellationToken);

            if (session == null)
            {
                return RequestResponse<string>.Fail("Session not found or unauthorized.");
            }

            if (session.Status == "Completed")
            {
                return RequestResponse<string>.Fail("Session is already completed.");
            }

            session.Status = "Completed";
            session.EndedAt = DateTime.UtcNow;

            _sessionRepository.Update(session);
            await _unitOfWork.SaveChangesAsync();

            await _publishEndpoint.Publish<IWorkoutSessionCompleted>(new
            {
                SessionId = session.Id.ToString(),
                WorkoutId = session.WorkoutId,
                UserId = session.UserId,
                DurationMinutes = request.DurationMinutes,
                TotalCaloriesBurned = request.CaloriesBurned,
                CompletedAt = session.EndedAt.Value
            }, cancellationToken);

            return RequestResponse<string>.Success(session.Id.ToString(), "Workout session completed successfully.");
        }
    }
}
