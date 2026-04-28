using Mapster;
using MediatR;
using MassTransit; // ✅ Required for Messaging
using WorkoutService.Contracts; // ✅ Required for Contracts
using WorkoutService.Features.Shared;
using WorkoutService.Features.Workouts.StartWorkoutSession.ViewModels;
using WorkoutService.Domain.Interfaces; // ✅ Required for ICurrentUserService
using WorkoutService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace WorkoutService.Features.Workouts.StartWorkoutSession
{
    public class StartWorkoutSessionCommandHandler : IRequestHandler<StartWorkoutSessionCommand, RequestResponse<WorkoutSessionViewModel>>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ICurrentUserService _currentUserService; // ✅ 1. Inject CurrentUserService
        private readonly IBaseRepository<Workout> _workoutRepository;
        private readonly IBaseRepository<WorkoutSession> _sessionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public StartWorkoutSessionCommandHandler(
            IPublishEndpoint publishEndpoint,
            ICurrentUserService currentUserService,
            IBaseRepository<Workout> workoutRepository,
            IBaseRepository<WorkoutSession> sessionRepository,
            IUnitOfWork unitOfWork)
        {
            _publishEndpoint = publishEndpoint;
            _currentUserService = currentUserService;
            _workoutRepository = workoutRepository;
            _sessionRepository = sessionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResponse<WorkoutSessionViewModel>> Handle(StartWorkoutSessionCommand request, CancellationToken cancellationToken)
        {
            if (!_currentUserService.IsAuthenticated || string.IsNullOrEmpty(_currentUserService.UserId))
            {
                return RequestResponse<WorkoutSessionViewModel>.Fail("User is not authenticated");
            }

            var userIdStr = _currentUserService.UserId;
            if (!Guid.TryParse(userIdStr, out Guid userIdGuid))
            {
                 return RequestResponse<WorkoutSessionViewModel>.Fail("Invalid user token format.");
            }

            var startedAt = DateTime.UtcNow;

            var workout = await _workoutRepository.GetAll()
                .Include(w => w.WorkoutExercises)
                .ThenInclude(we => we.Exercise)
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.Id == request.WorkoutId, cancellationToken);

            if (workout == null)
            {
                return RequestResponse<WorkoutSessionViewModel>.Fail("Workout not found.");
            }

            var session = new WorkoutSession
            {
                WorkoutId = request.WorkoutId,
                UserId = userIdGuid,
                Status = "InProgress",
                StartedAt = startedAt,
                PlannedDurationInMinutes = request.Dto.PlannedDuration,
                Difficulty = request.Dto.Difficulty,
                SessionExercises = workout.WorkoutExercises.Select(we => new WorkoutSessionExercise
                {
                    ExerciseId = we.ExerciseId,
                    Order = we.Order,
                    Status = "Pending",
                    Sets = we.Sets,
                    Reps = we.Reps,
                    RestTimeInSeconds = we.RestTimeInSeconds
                }).ToList()
            };

            await _sessionRepository.AddAsync(session);
            await _unitOfWork.SaveChangesAsync();

            await _publishEndpoint.Publish<IWorkoutSessionStarted>(new
            {
                WorkoutId = request.WorkoutId,
                UserId = userIdGuid,
                PlannedDurationMinutes = request.Dto.PlannedDuration,
                Difficulty = request.Dto.Difficulty,
                StartedAt = startedAt
            }, cancellationToken);

            var responseVm = new WorkoutSessionViewModel
            {
                SessionId = session.Id.ToString(), // Real Session ID!
                WorkoutId = request.WorkoutId,
                WorkoutName = workout.Name,
                status = "InProgress",
                PlannedDuration = request.Dto.PlannedDuration,
                Difficulty = request.Dto.Difficulty,
                StartedAt = startedAt,
                Exercises = session.SessionExercises.Select(se => new SessionExerciseViewModel
                {
                    Id = se.Id,
                    Name = workout.WorkoutExercises.FirstOrDefault(we => we.ExerciseId == se.ExerciseId)?.Exercise?.Name ?? "Unknown",
                    Order = se.Order,
                    Sets = se.Sets,
                    Reps = se.Reps,
                    RestTime = se.RestTimeInSeconds,
                    Completed = false
                }).ToList()
            };

            return RequestResponse<WorkoutSessionViewModel>.Success(responseVm, "Workout session started successfully.");
        }
    }
}