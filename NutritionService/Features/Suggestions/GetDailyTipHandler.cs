using MediatR;

namespace NutritionService.Features.Suggestions
{
    public class GetDailyTipHandler : IRequestHandler<GetDailyTipQuery, DailyTipDto>
    {
        public Task<DailyTipDto> Handle(GetDailyTipQuery request, CancellationToken cancellationToken)
        {
            var tip = FitnessTipsData.GetTip(request.Category);
            return Task.FromResult(tip);
        }
    }

    public static class FitnessTipsData
    {
        public static readonly List<DailyTipDto> Tips = new()
        {
            new() { Category = "nutrition", Title = "Protein", Tip = "Aim for 0.8-1g of protein per pound of body weight." },
            new() { Category = "nutrition", Title = "Water", Tip = "Drink at least 8 glasses of water daily." },
            new() { Category = "nutrition", Title = "Meal Prep", Tip = "Prep meals on Sunday to stay on track." },
            new() { Category = "nutrition", Title = "Breakfast", Tip = "Never skip breakfast - it kickstarts your metabolism." },
            new() { Category = "nutrition", Title = "Fiber", Tip = "Eat fiber-rich foods to stay full longer." },
            new() { Category = "nutrition", Title = "Portion Control", Tip = "Use smaller plates to control portion sizes." },
            new() { Category = "nutrition", Title = "Eat Colorful", Tip = "Fill your plate with colorful vegetables." },
            new() { Category = "workout", Title = "Warm Up", Tip = "Always warm up for 5-10 minutes before workouts." },
            new() { Category = "workout", Title = "Rest", Tip = "Take 48-72 hours between intense workouts." },
            new() { Category = "workout", Title = "Progressive Overload", Tip = "Gradually increase weight or reps each week." },
            new() { Category = "workout", Title = "Form First", Tip = "Perfect your form before adding heavy weights." },
            new() { Category = "workout", Title = "Compound Moves", Tip = "Focus on compound movements like squats." },
            new() { Category = "workout", Title = "Consistency", Tip = "Consistency beats intensity." },
            new() { Category = "workout", Title = "Home Workout", Tip = "Bodyweight exercises can be very effective." },
            new() { Category = "workout", Title = "HIIT", Tip = "15-20 minutes of HIIT can torch calories." },
            new() { Category = "recovery", Title = "Sleep", Tip = "Get 7-9 hours of sleep per night." },
            new() { Category = "recovery", Title = "Stretching", Tip = "Stretch after workouts for flexibility." },
            new() { Category = "recovery", Title = "Foam Roll", Tip = "Foam rolling helps reduce muscle tension." },
            new() { Category = "recovery", Title = "Rest Days", Tip = "Muscles grow during rest, not workouts." },
            new() { Category = "mindset", Title = "Goal Setting", Tip = "Set specific, measurable goals." },
            new() { Category = "mindset", Title = "Celebrate Wins", Tip = "Celebrate small wins to stay motivated." },
            new() { Category = "mindset", Title = "Accountability", Tip = "Find a workout buddy for accountability." },
            new() { Category = "mindset", Title = "Morning Routine", Tip = "Workout first thing in the morning." }
        };

        public static DailyTipDto GetTip(string? category = null)
        {
            var availableTips = string.IsNullOrEmpty(category)
                ? Tips
                : Tips.Where(t => t.Category.ToLower() == category.ToLower()).ToList();

            if (!availableTips.Any())
                availableTips = Tips;

            var dayOfYear = DateTime.Now.DayOfYear;
            var index = dayOfYear % availableTips.Count;

            return availableTips[index];
        }
    }
}