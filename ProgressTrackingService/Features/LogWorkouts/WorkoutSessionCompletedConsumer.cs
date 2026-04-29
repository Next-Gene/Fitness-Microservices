using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using ProgressTrackingService.Features.LogWorkout;
using WorkoutService.Contracts;
using System;
using System.Threading.Tasks;

namespace ProgressTrackingService.Features.Workouts.Consumers
{
    public class WorkoutSessionCompletedConsumer : IConsumer<IWorkoutSessionCompleted>
    {
        private readonly ISender _sender;
        private readonly ILogger<WorkoutSessionCompletedConsumer> _logger;

        public WorkoutSessionCompletedConsumer(ISender sender, ILogger<WorkoutSessionCompletedConsumer> logger)
        {
            _sender = sender;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IWorkoutSessionCompleted> context)
        {
            _logger.LogInformation("Received WorkoutSessionCompleted for SessionId: {SessionId}", context.Message.SessionId);

            Guid sessionIdGuid;
            if (int.TryParse(context.Message.SessionId, out var sessionIdInt))
            {
                byte[] bytes = new byte[16];
                BitConverter.GetBytes(sessionIdInt).CopyTo(bytes, 0);
                sessionIdGuid = new Guid(bytes);
            }
            else if (!Guid.TryParse(context.Message.SessionId, out sessionIdGuid))
            {
                _logger.LogWarning("Invalid SessionId format: {SessionId}", context.Message.SessionId);
                return;
            }

            var command = new LogWorkoutCommand(
                UserId: context.Message.UserId,
                SessionId: sessionIdGuid,
                WorkoutId: null, // WorkoutId from WorkoutService is an int, but ProgressTracking expects Guid
                DurationMinutes: context.Message.DurationMinutes,
                CaloriesBurned: context.Message.TotalCaloriesBurned,
                Rating: 0, 
                PerformedAt: context.Message.CompletedAt,
                ClientRequestId: $"Event-{context.Message.SessionId}"
            );

            await _sender.Send(command);
        }
    }
}
