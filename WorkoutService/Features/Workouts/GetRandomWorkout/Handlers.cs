using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkoutService.Domain.Entities;
using WorkoutService.Infrastructure.Data;

namespace WorkoutService.Features.Workouts.GetRandomWorkout
{
    public class GetRandomWorkoutHandler : IRequestHandler<GetRandomWorkoutQuery, WorkoutSuggestionDto>
    {
        private readonly ApplicationDbContext _context;

        public GetRandomWorkoutHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WorkoutSuggestionDto> Handle(GetRandomWorkoutQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Workouts
                .Include(w => w.WorkoutExercises)
                .ThenInclude(we => we.Exercise)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.Category))
                query = query.Where(w => w.Category.ToLower() == request.Category.ToLower());

            if (!string.IsNullOrEmpty(request.Difficulty))
                query = query.Where(w => w.Difficulty.ToLower() == request.Difficulty.ToLower());

            if (request.MaxDuration.HasValue)
                query = query.Where(w => w.DurationInMinutes <= request.MaxDuration.Value);

            if (request.NoEquipment == true)
                query = query.Where(w => w.Category.ToLower().Contains("home") || w.Category.ToLower().Contains("bodyweight"));

            var count = await query.CountAsync(cancellationToken);
            if (count == 0)
                throw new Exception("No workouts found matching criteria.");

            var random = new Random();
            var randomSkip = random.Next(count);

            var workout = await query
                .Skip(randomSkip)
                .Take(1)
                .FirstOrDefaultAsync(cancellationToken);

            if (workout == null)
                workout = await query.FirstOrDefaultAsync(cancellationToken);

            return new WorkoutSuggestionDto
            {
                Id = workout.Id,
                Name = workout.Name,
                Description = workout.Description,
                Category = workout.Category,
                Difficulty = workout.Difficulty,
                DurationInMinutes = workout.DurationInMinutes,
                CaloriesBurn = workout.CaloriesBurn,
                Exercises = workout.WorkoutExercises?
                    .OrderBy(we => we.Order)
                    .Take(8)
                    .Select(we => new ExerciseSummary
                    {
                        Name = we.Exercise?.Name ?? "",
                        Sets = we.Sets,
                        Reps = we.Reps ?? ""
                    })
                    .ToList() ?? new List<ExerciseSummary>()
            };
        }
    }
}