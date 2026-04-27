using Microsoft.EntityFrameworkCore;
using WorkoutService.Domain.Entities;
using WorkoutService.Infrastructure.Data;

namespace WorkoutService.Infrastructure.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(IServiceProvider sp)
        {

            var ctx = sp.GetRequiredService<ApplicationDbContext>();
            ctx.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            try
            {
            // 1. Seed Plans (Parents)
            await SeedWorkoutPlansAsync(ctx);

            }
            catch (Exception ex)
            {
                // Log or handle exceptions as needed

                throw;
            }
            try
            {
            // 2. Seed Exercises (Parents)
            await SeedExercisesAsync(ctx);
            }
            catch (Exception ex)
            {
                // Log or handle exceptions as needed

                throw;
            }
            try
            {

            // 3. Seed Workouts (Children - depend on Plans and Exercises)
            await SeedWorkoutsAsync(ctx);
            }
            catch (Exception ex)
            {
                // Log or handle exceptions as needed
                throw;
            }
        }

        private static async Task SeedWorkoutPlansAsync(ApplicationDbContext ctx)
        {
            // If data exists, do nothing
            if (await ctx.WorkoutPlans.AnyAsync()) return;

            var plans = new List<WorkoutPlan>
            {
                // Note: Removed 'Id' assignment to let SQL Server handle Identity
                new() {
                    ExternalPlanId = "plan_lw_normal",
                    Name = "Weight Loss - Normal Intensity",
                    Description = "A balanced plan for steady weight loss.",
                    Goal = "Lose Weight",
                    Status = "Normal",
                    Difficulty = "Intermediate"
                },
                new() {
                    ExternalPlanId = "plan_gw_hard",
                    Name = "Gain Weight - Hard Intensity",
                    Description = "A high-volume plan for building mass.",
                    Goal = "Gain Weight",
                    Status = "Hard",
                    Difficulty = "Advanced"
                },
                new() {
                    ExternalPlanId = "plan_fit_beginner",
                    Name = "Get Fitter - Beginner",
                    Description = "An introduction to fitness fundamentals.",
                    Goal = "Get Fitter",
                    Status = "Normal",
                    Difficulty = "Beginner"
                },
                new() {
                    ExternalPlanId = "plan_fit_intermediate",
                    Name = "Get Fitter - Intermediate",
                    Description = "Step up your fitness with more challenging routines.",
                    Goal = "Get Fitter",
                    Status = "Normal",
                    Difficulty = "Intermediate"
                },
                new() {
                    ExternalPlanId = "plan_fit_advanced",
                    Name = "Get Fitter - Advanced",
                    Description = "Push your limits with high-intensity workouts.",
                    Goal = "Get Fitter",
                    Status = "Hard",
                    Difficulty = "Advanced"
                },
                new() {
                    ExternalPlanId = "plan_lw_easy",
                    Name = "Weight Loss - Easy Start",
                    Description = "A gentle introduction to weight loss exercises.",
                    Goal = "Lose Weight",
                    Status = "Easy",
                    Difficulty = "Beginner"
                },
                new() {
                    ExternalPlanId = "plan_lw_hard",
                    Name = "Weight Loss - High Intensity",
                    Description = "Intense cardio and strength training for maximum fat burn.",
                    Goal = "Lose Weight",
                    Status = "Hard",
                    Difficulty = "Advanced"
                },
                new() {
                    ExternalPlanId = "plan_gw_beginner",
                    Name = "Gain Weight - Beginner Bulk",
                    Description = "Fundamental strength exercises to start building muscle.",
                    Goal = "Gain Weight",
                    Status = "Normal",
                    Difficulty = "Beginner"
                },
                new() {
                    ExternalPlanId = "plan_gw_intermediate",
                    Name = "Gain Weight - Intermediate Mass",
                    Description = "Increase volume and intensity to pack on more size.",
                    Goal = "Gain Weight",
                    Status = "Normal",
                    Difficulty = "Intermediate"
                },
                new() {
                    ExternalPlanId = "plan_st_beginner",
                    Name = "Strength Training - Beginner",
                    Description = "Learn the core lifts and build a solid strength base.",
                    Goal = "Build Strength",
                    Status = "Normal",
                    Difficulty = "Beginner"
                },
                new() {
                    ExternalPlanId = "plan_st_intermediate",
                    Name = "Strength Training - Intermediate",
                    Description = "Intermediate programming for consistent strength gains.",
                    Goal = "Build Strength",
                    Status = "Normal",
                    Difficulty = "Intermediate"
                },
                new() {
                    ExternalPlanId = "plan_st_advanced",
                    Name = "Strength Training - Advanced Power",
                    Description = "Advanced techniques for experienced lifters.",
                    Goal = "Build Strength",
                    Status = "Hard",
                    Difficulty = "Advanced"
                },
                new() {
                    ExternalPlanId = "plan_flex_beginner",
                    Name = "Flexibility - Beginner Yoga",
                    Description = "Basic yoga poses to improve flexibility and reduce stress.",
                    Goal = "Improve Flexibility",
                    Status = "Easy",
                    Difficulty = "Beginner"
                },
                new() {
                    ExternalPlanId = "plan_flex_intermediate",
                    Name = "Flexibility - Dynamic Stretching",
                    Description = "Improve mobility with dynamic stretches and movements.",
                    Goal = "Improve Flexibility",
                    Status = "Normal",
                    Difficulty = "Intermediate"
                },
                new() {
                    ExternalPlanId = "plan_end_beginner",
                    Name = "Endurance - Cardio Starter",
                    Description = "Build your cardiovascular base with steady-state cardio.",
                    Goal = "Improve Endurance",
                    Status = "Easy",
                    Difficulty = "Beginner"
                },
                new() {
                    ExternalPlanId = "plan_end_intermediate",
                    Name = "Endurance - HIIT Cardio",
                    Description = "High-Intensity Interval Training to boost endurance and burn calories.",
                    Goal = "Improve Endurance",
                    Status = "Hard",
                    Difficulty = "Intermediate"
                },
                new() {
                    ExternalPlanId = "plan_end_advanced",
                    Name = "Endurance - Marathon Prep",
                    Description = "Advanced cardio training for long-distance events.",
                    Goal = "Improve Endurance",
                    Status = "Hard",
                    Difficulty = "Advanced"
                },
                new() {
                    ExternalPlanId = "plan_body_beginner",
                    Name = "Bodyweight - Beginner Basics",
                    Description = "Master the fundamentals of bodyweight training.",
                    Goal = "Bodyweight Fitness",
                    Status = "Normal",
                    Difficulty = "Beginner"
                },
                new() {
                    ExternalPlanId = "plan_body_advanced",
                    Name = "Bodyweight - Calisthenics Master",
                    Description = "Advanced calisthenics skills and progressions.",
                    Goal = "Bodyweight Fitness",
                    Status = "Hard",
                    Difficulty = "Advanced"
                },
                new() {
                    ExternalPlanId = "plan_mind_beginner",
                    Name = "Mind & Body - Meditation",
                    Description = "Guided meditation for focus and stress relief.",
                    Goal = "Mindfulness",
                    Status = "Easy",
                    Difficulty = "Beginner"
                },
                 new() {
                    ExternalPlanId = "plan_rec_active",
                    Name = "Active Recovery",
                    Description = "Low-intensity workouts to aid recovery and reduce soreness.",
                    Goal = "Recovery",
                    Status = "Easy",
                    Difficulty = "Beginner"
                },
                // ===== HOME WORKOUT PLANS =====
                new() {
                    ExternalPlanId = "plan_home_beginner",
                    Name = "Home Workout - Beginner",
                    Description = "No equipment needed. Perfect for home workouts.",
                    Goal = "Get Fitter",
                    Status = "Easy",
                    Difficulty = "Beginner"
                },
                new() {
                    ExternalPlanId = "plan_home_intermediate",
                    Name = "Home Workout - Intermediate",
                    Description = "More challenging bodyweight exercises for home.",
                    Goal = "Get Fitter",
                    Status = "Normal",
                    Difficulty = "Intermediate"
                },
                new() {
                    ExternalPlanId = "plan_home_hiit",
                    Name = "Home HIIT - Quick Sweat",
                    Description = "High-intensity interval training at home.",
                    Goal = "Lose Weight",
                    Status = "Hard",
                    Difficulty = "Intermediate"
                },
                new() {
                    ExternalPlanId = "plan_quick_15",
                    Name = "15-Minute Quick Sweat",
                    Description = "Quick workouts for busy schedules.",
                    Goal = "Get Fitter",
                    Status = "Easy",
                    Difficulty = "Beginner"
                }
            };

            await ctx.WorkoutPlans.AddRangeAsync(plans);
            await ctx.SaveChangesAsync();
        }

        private static async Task SeedExercisesAsync(ApplicationDbContext ctx)
        {
            if (await ctx.Exercises.AnyAsync()) return;

            var exercises = new List<Exercise>
            {
                // Note: Removed 'Id' assignment here as well
                new() {
                    Name = "Push-up",
                    Description = "A basic calisthenic exercise for upper body strength.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Chest", "Triceps", "Shoulders" },
                    EquipmentNeeded = new List<string> { "Bodyweight" }
                },
                new() {
                    Name = "Bodyweight Squat",
                    Description = "A fundamental lower body exercise.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Quads", "Glutes", "Hamstrings" },
                    EquipmentNeeded = new List<string> { "Bodyweight" }
                },
                new() {
                    Name = "Plank",
                    Description = "An isometric core strength exercise.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Core", "Abs" },
                    EquipmentNeeded = new List<string> { "Bodyweight" }
                },
                new() {
                    Name = "Dumbbell Bench Press",
                    Description = "A chest-building exercise using dumbbells.",
                    Difficulty = "Intermediate",
                    TargetMuscles = new List<string> { "Chest", "Triceps" },
                    EquipmentNeeded = new List<string> { "Dumbbells", "Bench" }
                },
                new() {
                    Name = "Dumbbell Row",
                    Description = "A back-building exercise using a single dumbbell.",
                    Difficulty = "Intermediate",
                    TargetMuscles = new List<string> { "Back", "Lats", "Biceps" },
                    EquipmentNeeded = new List<string> { "Dumbbells", "Bench" }
                },
                // Chest Exercises
                new() {
                    Name = "Incline Dumbbell Press",
                    Description = "Targets the upper chest muscles.",
                    Difficulty = "Intermediate",
                    TargetMuscles = new List<string> { "Chest", "Shoulders", "Triceps" },
                    EquipmentNeeded = new List<string> { "Dumbbells", "Incline Bench" }
                },
                new() {
                    Name = "Barbell Bench Press",
                    Description = "The classic lift for chest, shoulders, and triceps.",
                    Difficulty = "Intermediate",
                    TargetMuscles = new List<string> { "Chest", "Shoulders", "Triceps" },
                    EquipmentNeeded = new List<string> { "Barbell", "Bench" }
                },
                new() {
                    Name = "Cable Crossover",
                    Description = "An isolation exercise for the chest.",
                    Difficulty = "Intermediate",
                    TargetMuscles = new List<string> { "Chest" },
                    EquipmentNeeded = new List<string> { "Cable Machine" }
                },
                // Back Exercises
                new() {
                    Name = "Pull-up",
                    Description = "A challenging bodyweight exercise for the back and biceps.",
                    Difficulty = "Advanced",
                    TargetMuscles = new List<string> { "Back", "Lats", "Biceps" },
                    EquipmentNeeded = new List<string> { "Pull-up Bar" }
                },
                new() {
                    Name = "Lat Pulldown",
                    Description = "A machine-based alternative to pull-ups.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Back", "Lats" },
                    EquipmentNeeded = new List<string> { "Lat Pulldown Machine" }
                },
                new() {
                    Name = "Deadlift",
                    Description = "A full-body lift that heavily targets the back and legs.",
                    Difficulty = "Advanced",
                    TargetMuscles = new List<string> { "Back", "Glutes", "Hamstrings" },
                    EquipmentNeeded = new List<string> { "Barbell" }
                },
                // Leg Exercises
                new() {
                    Name = "Barbell Squat",
                    Description = "The king of leg exercises, targeting the entire lower body.",
                    Difficulty = "Advanced",
                    TargetMuscles = new List<string> { "Quads", "Glutes", "Hamstrings" },
                    EquipmentNeeded = new List<string> { "Barbell", "Squat Rack" }
                },
                new() {
                    Name = "Leg Press",
                    Description = "A machine-based exercise for lower body strength.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Quads", "Glutes" },
                    EquipmentNeeded = new List<string> { "Leg Press Machine" }
                },
                new() {
                    Name = "Lunge",
                    Description = "A unilateral exercise for balance and leg strength.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Quads", "Glutes" },
                    EquipmentNeeded = new List<string> { "Bodyweight", "Dumbbells" }
                },
                new() {
                    Name = "Romanian Deadlift",
                    Description = "Targets the hamstrings and glutes.",
                    Difficulty = "Intermediate",
                    TargetMuscles = new List<string> { "Hamstrings", "Glutes" },
                    EquipmentNeeded = new List<string> { "Barbell", "Dumbbells" }
                },
                 new() {
                    Name = "Calf Raise",
                    Description = "An isolation exercise for the calf muscles.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Calves" },
                    EquipmentNeeded = new List<string> { "Bodyweight", "Dumbbells" }
                },
                // Shoulder Exercises
                new() {
                    Name = "Overhead Press",
                    Description = "A compound movement for shoulder strength.",
                    Difficulty = "Intermediate",
                    TargetMuscles = new List<string> { "Shoulders", "Triceps" },
                    EquipmentNeeded = new List<string> { "Barbell", "Dumbbells" }
                },
                new() {
                    Name = "Lateral Raise",
                    Description = "Isolates the side deltoids for broader shoulders.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Shoulders" },
                    EquipmentNeeded = new List<string> { "Dumbbells" }
                },
                new() {
                    Name = "Face Pull",
                    Description = "Excellent for shoulder health and rear deltoid development.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Shoulders", "Upper Back" },
                    EquipmentNeeded = new List<string> { "Cable Machine", "Resistance Bands" }
                },
                // Arm Exercises
                new() {
                    Name = "Bicep Curl",
                    Description = "The classic exercise for building biceps.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Biceps" },
                    EquipmentNeeded = new List<string> { "Dumbbells", "Barbell" }
                },
                // ===== BEGINNER HOME EXERCISES =====
                new() {
                    Name = "Wall Push-up",
                    Description = "An easier push-up variation using a wall for support.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Chest", "Triceps" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Knee Push-up",
                    Description = "Push-up from the knees for beginners.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Chest", "Triceps" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Chair Squat",
                    Description = "Squat to a chair for support and form guidance.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Quads", "Glutes" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Glute Bridge",
                    Description = "Floor exercise targeting the glutes and hamstrings.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Glutes", "Hamstrings" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Bird Dog",
                    Description = "Core and balance exercise on all fours.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Core", "Back" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Marching in Place",
                    Description = "Low-impact cardio to get moving.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Cardio" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "High Knees",
                    Description = "Running in place with high knee lifts.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Cardio", "Quads" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Mountain Climber",
                    Description = "Dynamic core and cardio exercise.",
                    Difficulty = "Intermediate",
                    TargetMuscles = new List<string> { "Core", "Cardio" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Bicycle Crunch",
                    Description = "Effective ab exercise targeting obliques.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Abs", "Obliques" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Dead Bug",
                    Description = "Core stability exercise.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Core", "Abs" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Superman",
                    Description = "Back extension for lower back strength.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Back", "Glutes" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Wall Sit",
                    Description = "Isometric leg exercise against a wall.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Quads", "Glutes" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Calf Raise",
                    Description = "Standing calf raises without equipment.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Calves" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Lunge",
                    Description = "Unilateral leg exercise.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Quads", "Glutes" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Side Lunge",
                    Description = "Lateral movement for inner thighs.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Quads", "Inner Thighs" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Skater Hop",
                    Description = "Lateral plyometric exercise.",
                    Difficulty = "Intermediate",
                    TargetMuscles = new List<string> { "Quads", "Glutes", "Cardio" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Squat Jump",
                    Description = "Explosive lower body exercise.",
                    Difficulty = "Intermediate",
                    TargetMuscles = new List<string> { "Quads", "Glutes", "Cardio" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                // ===== MORE EQUIPMENT-FREE EXERCISES =====
                new() {
                    Name = "Bodyweight Row (Towel)",
                    Description = "Towel row for back development.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Back", "Biceps" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Inchworm",
                    Description = "Full body stretch and core exercise.",
                    Difficulty = "Intermediate",
                    TargetMuscles = new List<string> { "Core", "Shoulders" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Pike Push-up",
                    Description = "Advanced push-up variation.",
                    Difficulty = "Intermediate",
                    TargetMuscles = new List<string> { "Chest", "Shoulders", "Triceps" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Diamond Push-up",
                    Description = "Close-grip push-up for triceps.",
                    Difficulty = "Intermediate",
                    TargetMuscles = new List<string> { "Triceps", "Chest" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Wide Push-up",
                    Description = "Wide-grip push-up for chest.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Chest" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Tricep Dip (Chair)",
                    Description = "Tricep dip using a chair.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Triceps", "Shoulders" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Single-Leg Squat",
                    Description = "Pistol squat progression.",
                    Difficulty = "Advanced",
                    TargetMuscles = new List<string> { "Quads", "Glutes" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Sumo Squat",
                    Description = "Wide-stance squat.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Quads", "Glutes", "Inner Thighs" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Jump Squat",
                    Description = "Explosive squat jump.",
                    Difficulty = "Intermediate",
                    TargetMuscles = new List<string> { "Quads", "Glutes", "Cardio" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Curtsey Lunge",
                    Description = "Lateral lunge variation.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Quads", "Glutes" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Reverse Lunge",
                    Description = "Lunge stepping back.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Quads", "Glutes" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Hip Thrust",
                    Description = "Floor hip thrust for glutes.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Glutes" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Donkey Kick",
                    Description = "Glute kickback.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Glutes" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Fire Hydrant",
                    Description = "Hip abduction exercise.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Glutes", "Outer Thighs" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Heel Tap",
                    Description = "Core exercise for obliques.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Obliques", "Core" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "V-Up",
                    Description = "Advanced core exercise.",
                    Difficulty = "Advanced",
                    TargetMuscles = new List<string> { "Abs", "Core" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Toe Touch",
                    Description = "Standing core exercise.",
                    Difficulty = "Intermediate",
                    TargetMuscles = new List<string> { "Abs", "Core" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Flutter Kick",
                    Description = "Core and hip flexor exercise.",
                    Difficulty = "Intermediate",
                    TargetMuscles = new List<string> { "Abs", "Hip Flexors" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Plank Tap",
                    Description = "Plank with arm taps.",
                    Difficulty = "Intermediate",
                    TargetMuscles = new List<string> { "Core", "Shoulders" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Bear Crawl",
                    Description = "Crawling core exercise.",
                    Difficulty = "Intermediate",
                    TargetMuscles = new List<string> { "Core", "Shoulders" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Squat Hold",
                    Description = "Isometric squat hold.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Quads", "Glutes" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Pulse Squat",
                    Description = "Small pulse in squat position.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Quads", "Glutes" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Isometric Wall Sit",
                    Description = "Wall sit hold.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Quads" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Clapping Jack",
                    Description = "Jumping jacks with claps.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Cardio" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Butt Kicks",
                    Description = "Running in place kicking heels to glutes.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Cardio", "Hamstrings" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Skaters",
                    Description = "Lateral skating movement.",
                    Difficulty = "Intermediate",
                    TargetMuscles = new List<string> { "Cardio", "Glutes" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Fast Feet",
                    Description = "Quick feet shuffling.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Cardio" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Square Jump",
                    Description = "Jump in four directions.",
                    Difficulty = "Intermediate",
                    TargetMuscles = new List<string> { "Cardio", "Quads" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Plank Jack",
                    Description = "Plank with jumping jacks.",
                    Difficulty = "Intermediate",
                    TargetMuscles = new List<string> { "Core", "Cardio" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Tuck Jump",
                    Description = "Jump with knees to chest.",
                    Difficulty = "Advanced",
                    TargetMuscles = new List<string> { "Cardio", "Quads" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Star Jump",
                    Description = "Jump with limbs extended.",
                    Difficulty = "Intermediate",
                    TargetMuscles = new List<string> { "Cardio" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                // ===== GYM EXERCISES =====
                new() {
                    Name = "Tricep Pushdown",
                    Description = "Isolates the triceps using a cable machine.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Triceps" },
                    EquipmentNeeded = new List<string> { "Cable Machine" }
                },
                new() {
                    Name = "Skull Crusher",
                    Description = "An effective exercise for tricep mass.",
                    Difficulty = "Intermediate",
                    TargetMuscles = new List<string> { "Triceps" },
                    EquipmentNeeded = new List<string> { "EZ Bar", "Dumbbells", "Bench" }
                },
                new() {
                    Name = "Hammer Curl",
                    Description = "Targets the biceps and brachialis for thicker arms.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Biceps", "Forearms" },
                    EquipmentNeeded = new List<string> { "Dumbbells" }
                },
                // Core Exercises
                new() {
                    Name = "Leg Raise",
                    Description = "Targets the lower abs.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Abs", "Core" },
                    EquipmentNeeded = new List<string> { "Bodyweight" }
                },
                new() {
                    Name = "Russian Twist",
                    Description = "Works the obliques and overall core stability.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Abs", "Obliques", "Core" },
                    EquipmentNeeded = new List<string> { "Bodyweight", "Kettlebell", "Dumbbell" }
                },
                new() {
                    Name = "Ab Wheel Rollout",
                    Description = "An advanced core exercise for serious abdominal strength.",
                    Difficulty = "Advanced",
                    TargetMuscles = new List<string> { "Abs", "Core", "Shoulders" },
                    EquipmentNeeded = new List<string> { "Ab Wheel" }
                },
                // Cardio
                new() {
                    Name = "Running",
                    Description = "Classic cardiovascular exercise.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Cardio" },
                    EquipmentNeeded = new List<string> { "None" }
                },
                new() {
                    Name = "Jumping Jacks",
                    Description = "A full-body cardio warm-up.",
                    Difficulty = "Beginner",
                    TargetMuscles = new List<string> { "Cardio" },
                    EquipmentNeeded = new List<string> { "Bodyweight" }
                },
                new() {
                    Name = "Burpee",
                    Description = "A high-intensity, full-body cardio and strength exercise.",
                    Difficulty = "Intermediate",
                    TargetMuscles = new List<string> { "Cardio", "Full Body" },
                    EquipmentNeeded = new List<string> { "Bodyweight" }
                }
            };

            await ctx.Exercises.AddRangeAsync(exercises);
            await ctx.SaveChangesAsync();
        }

        private static async Task SeedWorkoutsAsync(ApplicationDbContext ctx)
        {
            if (await ctx.Workouts.AnyAsync()) return;

            // Fetch created plans/exercises from DB to ensure valid IDs and Tracking
            var planBeginner = await ctx.WorkoutPlans.FirstOrDefaultAsync(p => p.ExternalPlanId == "plan_fit_beginner");
            var planNormal = await ctx.WorkoutPlans.FirstOrDefaultAsync(p => p.ExternalPlanId == "plan_lw_normal");
            var planFitIntermediate = await ctx.WorkoutPlans.FirstOrDefaultAsync(p => p.ExternalPlanId == "plan_fit_intermediate");
            var planFitAdvanced = await ctx.WorkoutPlans.FirstOrDefaultAsync(p => p.ExternalPlanId == "plan_fit_advanced");
            var planLwEasy = await ctx.WorkoutPlans.FirstOrDefaultAsync(p => p.ExternalPlanId == "plan_lw_easy");
            var planLwHard = await ctx.WorkoutPlans.FirstOrDefaultAsync(p => p.ExternalPlanId == "plan_lw_hard");
            var planGwBeginner = await ctx.WorkoutPlans.FirstOrDefaultAsync(p => p.ExternalPlanId == "plan_gw_beginner");
            var planStAdvanced = await ctx.WorkoutPlans.FirstOrDefaultAsync(p => p.ExternalPlanId == "plan_st_advanced");
            var planFlexBeginner = await ctx.WorkoutPlans.FirstOrDefaultAsync(p => p.ExternalPlanId == "plan_flex_beginner");
            var planEndIntermediate = await ctx.WorkoutPlans.FirstOrDefaultAsync(p => p.ExternalPlanId == "plan_end_intermediate");
            var planBodyAdvanced = await ctx.WorkoutPlans.FirstOrDefaultAsync(p => p.ExternalPlanId == "plan_body_advanced");
            var planRecActive = await ctx.WorkoutPlans.FirstOrDefaultAsync(p => p.ExternalPlanId == "plan_rec_active");
            var planHomeBeginner = await ctx.WorkoutPlans.FirstOrDefaultAsync(p => p.ExternalPlanId == "plan_home_beginner");
            var planHomeIntermediate = await ctx.WorkoutPlans.FirstOrDefaultAsync(p => p.ExternalPlanId == "plan_home_intermediate");
            var planHomeHiit = await ctx.WorkoutPlans.FirstOrDefaultAsync(p => p.ExternalPlanId == "plan_home_hiit");
            var planQuick15 = await ctx.WorkoutPlans.FirstOrDefaultAsync(p => p.ExternalPlanId == "plan_quick_15");

            var pushup = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Push-up");
            var squat = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Bodyweight Squat");
            var plank = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Plank");
            var dbPress = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Dumbbell Bench Press");
            var dbRow = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Dumbbell Row");
            var inclineDbPress = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Incline Dumbbell Press");
            var barbellBench = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Barbell Bench Press");
            var cableCrossover = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Cable Crossover");
            var pullup = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Pull-up");
            var latPulldown = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Lat Pulldown");
            var deadlift = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Deadlift");
            var barbellSquat = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Barbell Squat");
            var legPress = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Leg Press");
            var lunge = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Lunge");
            var rdl = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Romanian Deadlift");
            var calfRaise = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Calf Raise");
            var overheadPress = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Overhead Press");
            var lateralRaise = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Lateral Raise");
            var facePull = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Face Pull");
            var bicepCurl = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Bicep Curl");
            var tricepPushdown = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Tricep Pushdown");
            var skullCrusher = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Skull Crusher");
            var hammerCurl = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Hammer Curl");
            var legRaise = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Leg Raise");
            var russianTwist = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Russian Twist");
            var abWheel = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Ab Wheel Rollout");
            var running = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Running");
            var jumpingJacks = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Jumping Jacks");
            var burpee = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Burpee");

            // Home exercises
            var wallPushup = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Wall Push-up");
            var kneePushup = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Knee Push-up");
            var chairSquat = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Chair Squat");
            var gluteBridge = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Glute Bridge");
            var birdDog = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Bird Dog");
            var marching = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Marching in Place");
            var highKnees = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "High Knees");
            var mountainClimber = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Mountain Climber");
            var bicycleCrunch = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Bicycle Crunch");
            var deadBug = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Dead Bug");
            var superman = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Superman");
            var wallSit = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Wall Sit");
            var squatJump = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Squat Jump");
            var sideLunge = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Side Lunge");
            var skaterHop = await ctx.Exercises.FirstOrDefaultAsync(e => e.Name == "Skater Hop");

            // Safety check
            if (planBeginner == null || planNormal == null || pushup == null) return;

            var workouts = new List<Workout>
            {
                new() {
                    Name = "Full Body Introduction",
                    Description = "A workout to learn the basic movements.",
                    Category = "full-body",
                    Difficulty = "Beginner",
                    DurationInMinutes = 20,
                    CaloriesBurn = 150,
                    IsPremium = false,
                    Rating = 0,
                    // Use the Object Reference, NOT ID
                    WorkoutPlan = planBeginner,
                    WorkoutExercises = new List<WorkoutExercise>
                    {
                        // Use the Object Reference, NOT ID
                        new() { Exercise = squat, Order = 1, Sets = 3, Reps = "8-12", RestTimeInSeconds = 60 },
                        new() { Exercise = pushup, Order = 2, Sets = 3, Reps = "5-10 (Knees OK)", RestTimeInSeconds = 60 },
                        new() { Exercise = plank, Order = 3, Sets = 3, Reps = "30s", RestTimeInSeconds = 45 }
                    }
                },
                new() {
                    Name = "Strength & Cardio Mix",
                    Description = "A routine to burn calories and build muscle.",
                    Category = "full-body",
                    Difficulty = "Intermediate",
                    DurationInMinutes = 45,
                    CaloriesBurn = 350,
                    IsPremium = false,
                    Rating = 0,
                    WorkoutPlan = planNormal,
                    WorkoutExercises = new List<WorkoutExercise>
                    {
                        new() { Exercise = squat, Order = 1, Sets = 3, Reps = "12-15", RestTimeInSeconds = 60 },
                        new() { Exercise = dbPress, Order = 2, Sets = 3, Reps = "10-12", RestTimeInSeconds = 60 },
                        new() { Exercise = dbRow, Order = 3, Sets = 3, Reps = "10-12 (each side)", RestTimeInSeconds = 60 },
                        new() { Exercise = plank, Order = 4, Sets = 3, Reps = "60s", RestTimeInSeconds = 45 }
                    }
                },
                new() {
                    Name = "Chest Annihilation",
                    Description = "A high-volume workout for serious chest development.",
                    Category = "chest",
                    Difficulty = "Advanced",
                    DurationInMinutes = 60,
                    CaloriesBurn = 450,
                    IsPremium = true,
                    Rating = 5,
                    WorkoutPlan = planStAdvanced,
                    WorkoutExercises = new List<WorkoutExercise>
                    {
                        new() { Exercise = barbellBench, Order = 1, Sets = 4, Reps = "6-8", RestTimeInSeconds = 90 },
                        new() { Exercise = inclineDbPress, Order = 2, Sets = 4, Reps = "8-10", RestTimeInSeconds = 75 },
                        new() { Exercise = cableCrossover, Order = 3, Sets = 3, Reps = "12-15", RestTimeInSeconds = 60 },
                        new() { Exercise = pushup, Order = 4, Sets = 3, Reps = "To Failure", RestTimeInSeconds = 60 }
                    }
                },
                new() {
                    Name = "Back Builder",
                    Description = "Develop a wide and thick back with these essential exercises.",
                    Category = "back",
                    Difficulty = "Intermediate",
                    DurationInMinutes = 50,
                    CaloriesBurn = 400,
                    IsPremium = false,
                    Rating = 4.5,
                    WorkoutPlan = planFitIntermediate,
                    WorkoutExercises = new List<WorkoutExercise>
                    {
                        new() { Exercise = pullup, Order = 1, Sets = 4, Reps = "As many as possible", RestTimeInSeconds = 90 },
                        new() { Exercise = dbRow, Order = 2, Sets = 3, Reps = "8-12 (each side)", RestTimeInSeconds = 75 },
                        new() { Exercise = latPulldown, Order = 3, Sets = 3, Reps = "10-12", RestTimeInSeconds = 60 },
                        new() { Exercise = facePull, Order = 4, Sets = 3, Reps = "15-20", RestTimeInSeconds = 45 }
                    }
                },
                new() {
                    Name = "Leg Day Burnout",
                    Description = "A challenging leg workout that will leave you sore for days.",
                    Category = "legs",
                    Difficulty = "Advanced",
                    DurationInMinutes = 75,
                    CaloriesBurn = 600,
                    IsPremium = true,
                    Rating = 5,
                    WorkoutPlan = planLwHard,
                    WorkoutExercises = new List<WorkoutExercise>
                    {
                        new() { Exercise = barbellSquat, Order = 1, Sets = 5, Reps = "5", RestTimeInSeconds = 120 },
                        new() { Exercise = legPress, Order = 2, Sets = 4, Reps = "10-12", RestTimeInSeconds = 90 },
                        new() { Exercise = rdl, Order = 3, Sets = 3, Reps = "8-10", RestTimeInSeconds = 75 },
                        new() { Exercise = lunge, Order = 4, Sets = 3, Reps = "12 (each leg)", RestTimeInSeconds = 60 },
                        new() { Exercise = calfRaise, Order = 5, Sets = 4, Reps = "15-20", RestTimeInSeconds = 45 }
                    }
                },
                new() {
                    Name = "Shoulder Sculpt",
                    Description = "Build strong, rounded shoulders.",
                    Category = "shoulders",
                    Difficulty = "Intermediate",
                    DurationInMinutes = 40,
                    CaloriesBurn = 300,
                    IsPremium = false,
                    Rating = 4,
                    WorkoutPlan = planFitIntermediate,
                    WorkoutExercises = new List<WorkoutExercise>
                    {
                        new() { Exercise = overheadPress, Order = 1, Sets = 4, Reps = "8-10", RestTimeInSeconds = 75 },
                        new() { Exercise = lateralRaise, Order = 2, Sets = 3, Reps = "12-15", RestTimeInSeconds = 60 },
                        new() { Exercise = facePull, Order = 3, Sets = 3, Reps = "15-20", RestTimeInSeconds = 45 }
                    }
                },
                new() {
                    Name = "Arm Blaster",
                    Description = "A workout focused on building bigger biceps and triceps.",
                    Category = "arms",
                    Difficulty = "Intermediate",
                    DurationInMinutes = 35,
                    CaloriesBurn = 250,
                    IsPremium = false,
                    Rating = 4.2,
                    WorkoutPlan = planGwBeginner,
                    WorkoutExercises = new List<WorkoutExercise>
                    {
                        new() { Exercise = bicepCurl, Order = 1, Sets = 3, Reps = "10-12", RestTimeInSeconds = 60 },
                        new() { Exercise = tricepPushdown, Order = 2, Sets = 3, Reps = "10-12", RestTimeInSeconds = 60 },
                        new() { Exercise = hammerCurl, Order = 3, Sets = 3, Reps = "10-12", RestTimeInSeconds = 60 },
                        new() { Exercise = skullCrusher, Order = 4, Sets = 3, Reps = "10-12", RestTimeInSeconds = 75 }
                    }
                },
                new() {
                    Name = "Core Crusher",
                    Description = "A quick and intense workout to strengthen your core.",
                    Category = "stomach",
                    Difficulty = "Intermediate",
                    DurationInMinutes = 15,
                    CaloriesBurn = 100,
                    IsPremium = false,
                    Rating = 4.8,
                    WorkoutPlan = planFitIntermediate,
                    WorkoutExercises = new List<WorkoutExercise>
                    {
                        new() { Exercise = legRaise, Order = 1, Sets = 3, Reps = "15-20", RestTimeInSeconds = 45 },
                        new() { Exercise = russianTwist, Order = 2, Sets = 3, Reps = "20 (each side)", RestTimeInSeconds = 45 },
                        new() { Exercise = plank, Order = 3, Sets = 3, Reps = "60s hold", RestTimeInSeconds = 60 }
                    }
                },
                new() {
                    Name = "HIIT Cardio Blast",
                    Description = "High-Intensity Interval Training to torch calories and boost metabolism.",
                    Category = "full-body",
                    Difficulty = "Intermediate",
                    DurationInMinutes = 20,
                    CaloriesBurn = 300,
                    IsPremium = true,
                    Rating = 4.9,
                    WorkoutPlan = planEndIntermediate,
                    WorkoutExercises = new List<WorkoutExercise>
                    {
                        new() { Exercise = jumpingJacks, Order = 1, Sets = 1, Reps = "60s (Warm-up)", RestTimeInSeconds = 0 },
                        new() { Exercise = burpee, Order = 2, Sets = 4, Reps = "30s on, 30s off", RestTimeInSeconds = 30 },
                        new() { Exercise = squat, Order = 3, Sets = 4, Reps = "30s on, 30s off", RestTimeInSeconds = 30 },
                        new() { Exercise = pushup, Order = 4, Sets = 4, Reps = "30s on, 30s off", RestTimeInSeconds = 30 }
                    }
                },
                new() {
                    Name = "Beginner Bodyweight Circuit",
                    Description = "A simple circuit to get started with bodyweight exercises.",
                    Category = "full-body",
                    Difficulty = "Beginner",
                    DurationInMinutes = 25,
                    CaloriesBurn = 200,
                    IsPremium = false,
                    Rating = 4.3,
                    WorkoutPlan = planBeginner,
                    WorkoutExercises = new List<WorkoutExercise>
                    {
                        new() { Exercise = squat, Order = 1, Sets = 3, Reps = "10-15", RestTimeInSeconds = 60 },
                        new() { Exercise = pushup, Order = 2, Sets = 3, Reps = "5-10", RestTimeInSeconds = 60 },
                        new() { Exercise = lunge, Order = 3, Sets = 3, Reps = "10 (each leg)", RestTimeInSeconds = 60 },
                        new() { Exercise = plank, Order = 4, Sets = 3, Reps = "30-45s hold", RestTimeInSeconds = 45 }
                    }
                },
                new() {
                    Name = "Advanced Calisthenics",
                    Description = "Master your body with advanced bodyweight movements.",
                    Category = "full-body",
                    Difficulty = "Advanced",
                    DurationInMinutes = 60,
                    CaloriesBurn = 500,
                    IsPremium = true,
                    Rating = 5,
                    WorkoutPlan = planBodyAdvanced,
                    WorkoutExercises = new List<WorkoutExercise>
                    {
                        new() { Exercise = pullup, Order = 1, Sets = 5, Reps = "10-15", RestTimeInSeconds = 90 },
                        new() { Exercise = pushup, Order = 2, Sets = 5, Reps = "20-30", RestTimeInSeconds = 75 },
                        new() { Exercise = abWheel, Order = 3, Sets = 4, Reps = "10-15", RestTimeInSeconds = 60 },
                        new() { Exercise = burpee, Order = 4, Sets = 4, Reps = "15-20", RestTimeInSeconds = 60 }
                    }
                },
                new() {
                    Name = "Gentle Yoga Flow",
                    Description = "A relaxing yoga sequence to improve flexibility and mindfulness.",
                    Category = "flexibility",
                    Difficulty = "Beginner",
                    DurationInMinutes = 30,
                    CaloriesBurn = 100,
                    IsPremium = false,
                    Rating = 4.7,
                    WorkoutPlan = planFlexBeginner,
                    WorkoutExercises = new List<WorkoutExercise>
                    {
                        new() { Exercise = plank, Order = 1, Sets = 1, Reps = "Hold for 5 breaths", RestTimeInSeconds = 15 },
                        new() { Exercise = squat, Order = 2, Sets = 1, Reps = "Flow through 10 reps", RestTimeInSeconds = 15 },
                        new() { Exercise = lunge, Order = 3, Sets = 1, Reps = "Hold for 5 breaths each side", RestTimeInSeconds = 15 }
                    }
                },
                new() {
                    Name = "Active Recovery Day",
                    Description = "A light workout to help your muscles recover and grow.",
                    Category = "full-body",
                    Difficulty = "Beginner",
                    DurationInMinutes = 20,
                    CaloriesBurn = 100,
                    IsPremium = false,
                    Rating = 4.6,
                    WorkoutPlan = planRecActive,
                    WorkoutExercises = new List<WorkoutExercise>
                    {
                        new() { Exercise = running, Order = 1, Sets = 1, Reps = "10 minutes light jog", RestTimeInSeconds = 0 },
                        new() { Exercise = jumpingJacks, Order = 2, Sets = 2, Reps = "30s", RestTimeInSeconds = 30 },
                        new() { Exercise = plank, Order = 3, Sets = 2, Reps = "30s", RestTimeInSeconds = 30 }
                    }
                },

                // ===== HOME WORKOUTS =====
                new() {
                    Name = "No-Equipment Home Workout",
                    Description = "Complete workout with zero equipment. Perfect for home.",
                    Category = "Home Workout",
                    Difficulty = "Beginner",
                    DurationInMinutes = 30,
                    CaloriesBurn = 200,
                    IsPremium = false,
                    Rating = 4.7,
                    WorkoutPlan = planHomeBeginner,
                    WorkoutExercises = new List<WorkoutExercise>
                    {
                        new() { Exercise = wallPushup, Order = 1, Sets = 3, Reps = "10-15", RestTimeInSeconds = 60 },
                        new() { Exercise = chairSquat, Order = 2, Sets = 3, Reps = "15", RestTimeInSeconds = 60 },
                        new() { Exercise = gluteBridge, Order = 3, Sets = 3, Reps = "15", RestTimeInSeconds = 45 },
                        new() { Exercise = plank, Order = 4, Sets = 3, Reps = "30s", RestTimeInSeconds = 45 },
                        new() { Exercise = marching, Order = 5, Sets = 1, Reps = "5 minutes", RestTimeInSeconds = 0 }
                    }
                },
                new() {
                    Name = "Morning Mobility",
                    Description = "Gentle movements to start your day.",
                    Category = "Home Workout",
                    Difficulty = "Beginner",
                    DurationInMinutes = 15,
                    CaloriesBurn = 80,
                    IsPremium = false,
                    Rating = 4.8,
                    WorkoutPlan = planHomeBeginner,
                    WorkoutExercises = new List<WorkoutExercise>
                    {
                        new() { Exercise = marching, Order = 1, Sets = 1, Reps = "3 minutes", RestTimeInSeconds = 0 },
                        new() { Exercise = birdDog, Order = 2, Sets = 2, Reps = "10 each side", RestTimeInSeconds = 30 },
                        new() { Exercise = gluteBridge, Order = 3, Sets = 2, Reps = "10", RestTimeInSeconds = 30 },
                        new() { Exercise = superman, Order = 4, Sets = 2, Reps = "10", RestTimeInSeconds = 30 }
                    }
                },
                new() {
                    Name = "Living Room HIIT",
                    Description = "High-intensity workout in your living room.",
                    Category = "Home Workout",
                    Difficulty = "Intermediate",
                    DurationInMinutes = 25,
                    CaloriesBurn = 350,
                    IsPremium = false,
                    Rating = 4.9,
                    WorkoutPlan = planHomeHiit,
                    WorkoutExercises = new List<WorkoutExercise>
                    {
                        new() { Exercise = jumpingJacks, Order = 1, Sets = 4, Reps = "30s", RestTimeInSeconds = 15 },
                        new() { Exercise = highKnees, Order = 2, Sets = 4, Reps = "30s", RestTimeInSeconds = 15 },
                        new() { Exercise = mountainClimber, Order = 3, Sets = 4, Reps = "30s", RestTimeInSeconds = 15 },
                        new() { Exercise = squatJump, Order = 4, Sets = 4, Reps = "15", RestTimeInSeconds = 30 },
                        new() { Exercise = burpee, Order = 5, Sets = 3, Reps = "10", RestTimeInSeconds = 45 }
                    }
                },
                new() {
                    Name = "15-Minute Express",
                    Description = "Quick workout for busy days.",
                    Category = "Quick 15",
                    Difficulty = "Beginner",
                    DurationInMinutes = 15,
                    CaloriesBurn = 120,
                    IsPremium = false,
                    Rating = 4.5,
                    WorkoutPlan = planQuick15,
                    WorkoutExercises = new List<WorkoutExercise>
                    {
                        new() { Exercise = jumpingJacks, Order = 1, Sets = 1, Reps = "2 minutes", RestTimeInSeconds = 0 },
                        new() { Exercise = wallPushup, Order = 2, Sets = 3, Reps = "10", RestTimeInSeconds = 30 },
                        new() { Exercise = chairSquat, Order = 3, Sets = 3, Reps = "15", RestTimeInSeconds = 30 },
                        new() { Exercise = plank, Order = 4, Sets = 2, Reps = "30s", RestTimeInSeconds = 30 }
                    }
                },
                new() {
                    Name = "Core at Home",
                    Description = "No-equipment ab workout.",
                    Category = "Home Workout",
                    Difficulty = "Beginner",
                    DurationInMinutes = 20,
                    CaloriesBurn = 130,
                    IsPremium = false,
                    Rating = 4.6,
                    WorkoutPlan = planHomeBeginner,
                    WorkoutExercises = new List<WorkoutExercise>
                    {
                        new() { Exercise = plank, Order = 1, Sets = 3, Reps = "30s", RestTimeInSeconds = 30 },
                        new() { Exercise = bicycleCrunch, Order = 2, Sets = 3, Reps = "20", RestTimeInSeconds = 30 },
                        new() { Exercise = deadBug, Order = 3, Sets = 3, Reps = "10 each side", RestTimeInSeconds = 30 },
                        new() { Exercise = superman, Order = 4, Sets = 3, Reps = "15", RestTimeInSeconds = 30 },
                        new() { Exercise = gluteBridge, Order = 5, Sets = 3, Reps = "15", RestTimeInSeconds = 30 }
                    }
                },
                new() {
                    Name = "Advanced Bodyweight Burn",
                    Description = "Challenging bodyweight workout at home.",
                    Category = "Home Workout",
                    Difficulty = "Advanced",
                    DurationInMinutes = 45,
                    CaloriesBurn = 450,
                    IsPremium = true,
                    Rating = 5,
                    WorkoutPlan = planHomeIntermediate,
                    WorkoutExercises = new List<WorkoutExercise>
                    {
                        new() { Exercise = squatJump, Order = 1, Sets = 4, Reps = "15", RestTimeInSeconds = 60 },
                        new() { Exercise = mountainClimber, Order = 2, Sets = 4, Reps = "30s", RestTimeInSeconds = 30 },
                        new() { Exercise = burpee, Order = 3, Sets = 4, Reps = "15", RestTimeInSeconds = 60 },
                        new() { Exercise = skaterHop, Order = 4, Sets = 3, Reps = "20 each side", RestTimeInSeconds = 45 },
                        new() { Exercise = plank, Order = 5, Sets = 3, Reps = "60s", RestTimeInSeconds = 45 }
                    }
                },
                new() {
                    Name = "Lower Body Home",
                    Description = "Leg-focused home workout.",
                    Category = "Home Workout",
                    Difficulty = "Intermediate",
                    DurationInMinutes = 30,
                    CaloriesBurn = 280,
                    IsPremium = false,
                    Rating = 4.7,
                    WorkoutPlan = planHomeIntermediate,
                    WorkoutExercises = new List<WorkoutExercise>
                    {
                        new() { Exercise = squat, Order = 1, Sets = 4, Reps = "15", RestTimeInSeconds = 60 },
                        new() { Exercise = lunge, Order = 2, Sets = 3, Reps = "12 each leg", RestTimeInSeconds = 45 },
                        new() { Exercise = gluteBridge, Order = 3, Sets = 3, Reps = "15", RestTimeInSeconds = 30 },
                        new() { Exercise = wallSit, Order = 4, Sets = 3, Reps = "30s", RestTimeInSeconds = 45 },
                        new() { Exercise = sideLunge, Order = 5, Sets = 3, Reps = "12 each side", RestTimeInSeconds = 30 }
                    }
                }
            };

            await ctx.Workouts.AddRangeAsync(workouts);
            await ctx.SaveChangesAsync();
        }
    }
}