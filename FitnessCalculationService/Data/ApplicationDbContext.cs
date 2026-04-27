using FitnessCalculationService.Data;
using Microsoft.AspNetCore.Identity;

using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;


namespace FitnessCalculationService.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {




    }
    public DbSet<WorkoutPlandb> WorkoutPlans { get; set; }
    public DbSet<WeightGoalActivitydb> WeightGoalActivity { get; set; }
    public DbSet<UserFitnessStatdb> UserFitnessStat { get; set; }
    public DbSet<WorkoutPlandb> WorkoutPlan { get; set; }
    public DbSet<FitnessPlanConfigdb> FitnessPlanConfig { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Apply configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);



    }


    //DbSets

}
