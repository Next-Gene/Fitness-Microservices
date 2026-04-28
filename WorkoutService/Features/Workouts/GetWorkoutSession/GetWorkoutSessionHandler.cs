using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkoutService.Contracts;
using WorkoutService.Domain.Entities;
using WorkoutService.Domain.Interfaces;
using WorkoutService.Features.Shared;
using WorkoutService.Features.Workouts.StartWorkoutSession.ViewModels;

namespace WorkoutService.Features.Workouts.GetWorkoutSession
{
    public class GetWorkoutSessionHandler : IRequestHandler<GetWorkoutSessionQuery, RequestResponse<WorkoutSessionViewModel>>
    {
        private readonly IBaseRepository<WorkoutSession> _sessionRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetWorkoutSessionHandler(IBaseRepository<WorkoutSession> sessionRepository, ICurrentUserService currentUserService)
        {
            _sessionRepository = sessionRepository;
            _currentUserService = currentUserService;
        }

        public async Task<RequestResponse<WorkoutSessionViewModel>> Handle(GetWorkoutSessionQuery request, CancellationToken cancellationToken)
        {
            if (!_currentUserService.IsAuthenticated || string.IsNullOrEmpty(_currentUserService.UserId))
            {
                return RequestResponse<WorkoutSessionViewModel>.Fail("User is not authenticated");
            }

            if (!Guid.TryParse(_currentUserService.UserId, out Guid userIdGuid))
            {
                return RequestResponse<WorkoutSessionViewModel>.Fail("Invalid user token format.");
            }

            var session = await _sessionRepository.GetAll()
                .Include(s => s.Workout)
                .Include(s => s.SessionExercises)
                .ThenInclude(se => se.Exercise)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == request.SessionId && s.UserId == userIdGuid, cancellationToken);

            if (session == null)
            {
                return RequestResponse<WorkoutSessionViewModel>.Fail("Session not found or unauthorized.");
            }

            var responseVm = new WorkoutSessionViewModel
            {
                SessionId = session.Id.ToString(),
                WorkoutId = session.WorkoutId,
                WorkoutName = session.Workout?.Name ?? "Unknown Workout",
                status = session.Status,
                PlannedDuration = session.PlannedDurationInMinutes,
                Difficulty = session.Difficulty,
                StartedAt = session.StartedAt,
                Exercises = session.SessionExercises.OrderBy(se => se.Order).Select(se => new SessionExerciseViewModel
                {
                    Id = se.Id,
                    Name = se.Exercise?.Name ?? "Unknown Exercise",
                    Order = se.Order,
                    Sets = se.Sets,
                    Reps = se.Reps,
                    RestTime = se.RestTimeInSeconds,
                    Completed = se.Status == "Completed"
                }).ToList()
            };

            return RequestResponse<WorkoutSessionViewModel>.Success(responseVm, "Session retrieved successfully.");
        }
    }
}
