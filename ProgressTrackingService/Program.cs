
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProgressTrackingService.Api;
using ProgressTrackingService.Data;
using MassTransit;
using ProgressTrackingService.Features.Workouts.Consumers;

namespace ProgressTrackingService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // DbContext
            builder.Services.AddDbContext<ProgressDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // MediatR
            builder.Services.AddMediatR(typeof(Program).Assembly);

            // MemoryCache
            builder.Services.AddMemoryCache();
            builder.Services.AddHttpClient();

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // MassTransit Configuration
            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<WorkoutSessionCompletedConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    var rabbitMqHost = builder.Configuration["RabbitMq:Host"] ?? "localhost";
                    cfg.Host(rabbitMqHost, "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            var app = builder.Build();

            // Database Migration & Seeding
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ProgressDbContext>();
                    await context.Database.MigrateAsync();
                    await DatabaseSeeder.SeedAsync(services);
                }
                catch (Exception ex)
                {
                    // For minimal API, you can use app.Logger
                    app.Logger.LogError(ex, "An error occurred during migration or seeding.");
                }
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Map Minimal API endpoints
            app.MapProgressEndpoints();

            await app.RunAsync();
        }
    }
}
