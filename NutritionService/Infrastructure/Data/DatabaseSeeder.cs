using Microsoft.EntityFrameworkCore;
using NutritionService.Domain.Models;
using NutritionService.Domain.Models.Enums;
using NutritionService.Infrastructure.Data;

namespace NutritionService.Infrastructure.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(IServiceProvider sp)
        {
            var ctx = sp.GetRequiredService<ApplicationDbContext>();
            ctx.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;

            try
            {
                await SeedIngredientsAsync(ctx);
            }
            catch (Exception ex)
            {
                throw;
            }

            try
            {
                await SeedMealPlansAsync(ctx);
            }
            catch (Exception ex)
            {
                throw;
            }

            try
            {
                await SeedMealsAsync(ctx);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static async Task SeedIngredientsAsync(ApplicationDbContext ctx)
        {
            if (await ctx.ingredients.AnyAsync()) return;

            var ingredients = new List<Ingredient>
            {
// ===== PROTEINS =====
                new() { Name = "Chicken Breast" },
                new() { Name = "Salmon" },
                new() { Name = "Tuna" },
                new() { Name = "Eggs" },
                new() { Name = "Whey Protein" },
                new() { Name = "Greek Yogurt" },
                new() { Name = "Cottage Cheese" },
                new() { Name = "Turkey Breast" },
                new() { Name = "Beef Tenderloin" },
                new() { Name = "Tofu" },
                new() { Name = "Shrimp" },
                new() { Name = "Tilapia" },
                new() { Name = "Bacon" },
                new() { Name = "Pork Tenderloin" },
                new() { Name = "Duck Breast" },
                new() { Name = "Cod" },
                new() { Name = "Sardines" },
                new() { Name = "Mackerel" },
                new() { Name = "Sea Bass" },
                new() { Name = "Protein Powder" },
                new() { Name = "Casein Protein" },
                new() { Name = "Plant Protein" },
                new() { Name = "Tempeh" },
                new() { Name = "Edamame" },
                new() { Name = "Black Beans" },
                new() { Name = "Chickpeas" },
                new() { Name = "Lentils" },

                // ===== CARBS =====
                new() { Name = "Brown Rice" },
                new() { Name = "White Rice" },
                new() { Name = "Quinoa" },
                new() { Name = "Oats" },
                new() { Name = "Sweet Potato" },
                new() { Name = "White Potato" },
                new() { Name = "Whole Wheat Bread" },
                new() { Name = "Pasta" },
                new() { Name = "Banana" },
                new() { Name = "Apple" },
                new() { Name = "Blueberries" },
                new() { Name = "Strawberries" },
                new() { Name = "Rice Cakes" },
                new() { Name = "Basmati Rice" },
                new() { Name = "Jasmine Rice" },
                new() { Name = "Couscous" },
                new() { Name = "Bulgur" },
                new() { Name = "Polenta" },
                new() { Name = "Ezekiel Bread" },
                new() { Name = "Sourdough Bread" },
                new() { Name = "Bagel" },
                new() { Name = "Tortilla" },
                new() { Name = "English Muffin" },
                new() { Name = "Rice Noodles" },
                new() { Name = "Sweet Potato Noodles" },
                new() { Name = "Zucchini Noodles" },
                new() { Name = "Orange" },
                new() { Name = "Grapes" },
                new() { Name = "Mango" },
                new() { Name = "Pineapple" },
                new() { Name = "Watermelon" },
                new() { Name = "Peach" },
                new() { Name = "Pear" },
                new() { Name = "Cherries" },
                new() { Name = "Raspberries" },
                new() { Name = "Blackberries" },

                // ===== FATS =====
                new() { Name = "Olive Oil" },
                new() { Name = "Avocado" },
                new() { Name = "Almonds" },
                new() { Name = "Peanut Butter" },
                new() { Name = "Walnuts" },
                new() { Name = "Cheese" },
                new() { Name = "Butter" },
                new() { Name = "Coconut Oil" },
                new() { Name = "Almond Butter" },
                new() { Name = "Chia Seeds" },
                new() { Name = "Coconut" },
                new() { Name = "Cashews" },
                new() { Name = "Pistachios" },
                new() { Name = "Macadamia Nuts" },
                new() { Name = "Pecans" },
                new() { Name = "Hazelnuts" },
                new() { Name = "Sunflower Seeds" },
                new() { Name = "Pumpkin Seeds" },
                new() { Name = "Sesame Seeds" },
                new() { Name = "Flax Seeds" },
                new() { Name = "Hemp Seeds" },
                new() { Name = "Cream Cheese" },
                new() { Name = "Heavy Cream" },
                new() { Name = "Sour Cream" },
                new() { Name = "Ghee" },

                // ===== VEGETABLES =====
                new() { Name = "Broccoli" },
                new() { Name = "Spinach" },
                new() { Name = "Kale" },
                new() { Name = "Carrots" },
                new() { Name = "Bell Peppers" },
                new() { Name = "Zucchini" },
                new() { Name = "Cucumber" },
                new() { Name = "Tomatoes" },
                new() { Name = "Onions" },
                new() { Name = "Garlic" },
                new() { Name = "Mushrooms" },
                new() { Name = "Asparagus" },
                new() { Name = "Green Beans" },
                new() { Name = "Cauliflower" },
                new() { Name = "Celery" },
                new() { Name = "Lettuce" },
                new() { Name = "Arugula" },
                new() { Name = "Cabbage" },
                new() { Name = "Brussels Sprouts" },
                new() { Name = "Eggplant" },
                new() { Name = "Squash" },
                new() { Name = "Pumpkin" },
                new() { Name = "Beets" },
                new() { Name = "Radish" },
                new() { Name = "Turnip" },
                new() { Name = "Parsnip" },
                new() { Name = "Leek" },
                new() { Name = "Scallion" },
                new() { Name = "Ginger" },
                new() { Name = "Celery Root" },

                // ===== EXTRAS =====
                new() { Name = "Honey" },
                new() { Name = "Maple Syrup" },
                new() { Name = "Protein Powder" },
                new() { Name = "Milk" },
                new() { Name = "Almond Milk" },
                new() { Name = "Egg Whites" },
                new() { Name = "Coconut Milk" },
                new() { Name = "Oat Milk" },
                new() { Name = "Soy Milk" },
                new() { Name = "Rice Milk" },
                new() { Name = "Chicken Broth" },
                new() { Name = "Vegetable Broth" },
                new() { Name = "Coconut Amino" },
                new() { Name = "Soy Sauce" },
                new() { Name = "Tahini" },
                new() { Name = "Mustard" },
                new() { Name = "Hot Sauce" },
                new() { Name = "Sriracha" },
                new() { Name = "Salsa" },
                new() { Name = "Guacamole" },
                new() { Name = "Hummus" },
                new() { Name = "Pickles" },
                new() { Name = "Capers" },
                new() { Name = "Olives" },
                new() { Name = "Sundried Tomatoes" },
                new() { Name = "Artichokes" }
            };

            await ctx.ingredients.AddRangeAsync(ingredients);
            await ctx.SaveChangesAsync();
        }

        private static async Task SeedMealPlansAsync(ApplicationDbContext ctx)
        {
            if (await ctx.MealPlans.AnyAsync()) return;

var plans = new List<MealPlan>
            {
                // Weight Loss Plans (Low Calorie)
                new() { Name = "WL-1000", Description = "Weight Loss - 1000 kcal (Very Low)", CalorieTarget = 1000 },
                new() { Name = "WL-1200", Description = "Weight Loss - 1200 kcal", CalorieTarget = 1200 },
                new() { Name = "WL-1400", Description = "Weight Loss - 1400 kcal", CalorieTarget = 1400 },
                new() { Name = "WL-1600", Description = "Weight Loss - 1600 kcal", CalorieTarget = 1600 },
                new() { Name = "WL-1800", Description = "Weight Loss - 1800 kcal", CalorieTarget = 1800 },
                new() { Name = "WL-2000", Description = "Weight Loss - 2000 kcal", CalorieTarget = 2000 },

                // Weight Gain Plans (High Calorie)
                new() { Name = "GW-2200", Description = "Weight Gain - 2200 kcal", CalorieTarget = 2200 },
                new() { Name = "GW-2500", Description = "Weight Gain - 2500 kcal", CalorieTarget = 2500 },
                new() { Name = "GW-2800", Description = "Weight Gain - 2800 kcal", CalorieTarget = 2800 },
                new() { Name = "GW-3000", Description = "Weight Gain - 3000 kcal", CalorieTarget = 3000 },
                new() { Name = "GW-3500", Description = "Weight Gain - 3500 kcal", CalorieTarget = 3500 },
                new() { Name = "GW-4000", Description = "Weight Gain - 4000 kcal (Extreme)", CalorieTarget = 4000 },

                // Get Fitter Plans (Maintenance)
                new() { Name = "FIT-1500", Description = "Get Fitter - 1500 kcal", CalorieTarget = 1500 },
                new() { Name = "FIT-1800", Description = "Get Fitter - 1800 kcal", CalorieTarget = 1800 },
                new() { Name = "FIT-2000", Description = "Get Fitter - 2000 kcal", CalorieTarget = 2000 },
                new() { Name = "FIT-2200", Description = "Get Fitter - 2200 kcal", CalorieTarget = 2200 },
                new() { Name = "FIT-2500", Description = "Get Fitter - 2500 kcal", CalorieTarget = 2500 },
                new() { Name = "FIT-2800", Description = "Get Fitter - 2800 kcal", CalorieTarget = 2800 },

                // Special Plans
                new() { Name = "KETO-1500", Description = "Keto - 1500 kcal", CalorieTarget = 1500 },
                new() { Name = "KETO-2000", Description = "Keto - 2000 kcal", CalorieTarget = 2000 },
                new() { Name = "VEGAN-1800", Description = "Vegan - 1800 kcal", CalorieTarget = 1800 },
                new() { Name = "VEGAN-2200", Description = "Vegan - 2200 kcal", CalorieTarget = 2200 }
            };

            await ctx.MealPlans.AddRangeAsync(plans);
            await ctx.SaveChangesAsync();
        }

        private static async Task SeedMealsAsync(ApplicationDbContext ctx)
        {
            if (await ctx.meals.AnyAsync()) return;

            var plans = await ctx.MealPlans.ToListAsync();
            var wl1200 = plans.First(p => p.Name == "WL-1200");
            var wl1500 = plans.First(p => p.Name == "WL-1500");
            var wl1800 = plans.First(p => p.Name == "WL-1800");
            var gw2500 = plans.First(p => p.Name == "GW-2500");
            var gw3000 = plans.First(p => p.Name == "GW-3000");
            var gw3500 = plans.First(p => p.Name == "GW-3500");
            var fit2000 = plans.First(p => p.Name == "FIT-2000");
            var fit2500 = plans.First(p => p.Name == "FIT-2500");

            var ingredients = await ctx.ingredients.ToListAsync();

            var chickenBreast = ingredients.First(i => i.Name == "Chicken Breast");
            var salmon = ingredients.First(i => i.Name == "Salmon");
            var eggs = ingredients.First(i => i.Name == "Eggs");
            var greekYogurt = ingredients.First(i => i.Name == "Greek Yogurt");
            var cottageCheese = ingredients.First(i => i.Name == "Cottage Cheese");
            var turkeyBreast = ingredients.First(i => i.Name == "Turkey Breast");
            var wheyProtein = ingredients.First(i => i.Name == "Whey Protein");
            var tuna = ingredients.First(i => i.Name == "Tuna");
            var tofu = ingredients.First(i => i.Name == "Tofu");

            var brownRice = ingredients.First(i => i.Name == "Brown Rice");
            var whiteRice = ingredients.First(i => i.Name == "White Rice");
            var quinoa = ingredients.First(i => i.Name == "Quinoa");
            var oats = ingredients.First(i => i.Name == "Oats");
            var sweetPotato = ingredients.First(i => i.Name == "Sweet Potato");
            var whitePotato = ingredients.First(i => i.Name == "White Potato");
            var wholeWheatBread = ingredients.First(i => i.Name == "Whole Wheat Bread");
            var banana = ingredients.First(i => i.Name == "Banana");
            var apple = ingredients.First(i => i.Name == "Apple");
            var blueberries = ingredients.First(i => i.Name == "Blueberries");

            var oliveOil = ingredients.First(i => i.Name == "Olive Oil");
            var avocado = ingredients.First(i => i.Name == "Avocado");
            var almonds = ingredients.First(i => i.Name == "Almonds");
            var peanutButter = ingredients.First(i => i.Name == "Peanut Butter");
            var cheese = ingredients.First(i => i.Name == "Cheese");

            var broccoli = ingredients.First(i => i.Name == "Broccoli");
            var spinach = ingredients.First(i => i.Name == "Spinach");
            var kale = ingredients.First(i => i.Name == "Kale");
            var carrots = ingredients.First(i => i.Name == "Carrots");
            var bellPeppers = ingredients.First(i => i.Name == "Bell Peppers");
            var zucchini = ingredients.First(i => i.Name == "Zucchini");
            var cucumber = ingredients.First(i => i.Name == "Cucumber");
            var tomatoes = ingredients.First(i => i.Name == "Tomatoes");

            var honey = ingredients.First(i => i.Name == "Honey");
            var milk = ingredients.First(i => i.Name == "Milk");
            var almondMilk = ingredients.First(i => i.Name == "Almond Milk");

            var meals = new List<Meal>
            {
                // ===== WEIGHT LOSS 1200 CAL ===== (3 meals + 1 snack)
                new() {
                    Name = "Egg White Omelette",
                    Description = "Fluffy egg whites with fresh vegetables",
                    mealType = MealType.Breakfast,
                    Difficulty = "Easy",
                    PrepTimeInMinutes = 10,
                    ImageUrl = "https://images.unsplash.com/photo-1525351484163-7529414344d8?w=400",
                    IsPremium = false,
                    MealPlanId = wl1200.Id,
                    NutritionFacts = new NutritionFact { Calories = 250, Protein = 26, Carbs = 8, Fats = 12, Fiber = 3 },
                    MealIngredients = new List<MealIngredient>
                    {
                        new() { Ingredient = eggs, Amount = "150g" },
                        new() { Ingredient = spinach, Amount = "50g" },
                        new() { Ingredient = tomatoes, Amount = "30g" },
                        new() { Ingredient = oliveOil, Amount = "5g" }
                    }
                },
                new() {
                    Name = "Grilled Chicken Salad",
                    Description = "Lean protein with fresh greens",
                    mealType = MealType.Lunch,
                    Difficulty = "Easy",
                    PrepTimeInMinutes = 20,
                    ImageUrl = "https://images.unsplash.com/photo-1546069901-ba9599a7e63c?w=400",
                    IsPremium = false,
                    MealPlanId = wl1200.Id,
                    NutritionFacts = new NutritionFact { Calories = 400, Protein = 42, Carbs = 12, Fats = 18, Fiber = 5 },
                    MealIngredients = new List<MealIngredient>
                    {
                        new() { Ingredient = chickenBreast, Amount = "150g" },
                        new() { Ingredient = spinach, Amount = "80g" },
                        new() { Ingredient = cucumber, Amount = "50g" },
                        new() { Ingredient = tomatoes, Amount = "40g" },
                        new() { Ingredient = oliveOil, Amount = "10g" }
                    }
                },
                new() {
                    Name = "Baked Salmon with Asparagus",
                    Description = "Omega-3 rich fish with vegetables",
                    mealType = MealType.Dinner,
                    Difficulty = "Medium",
                    PrepTimeInMinutes = 25,
                    ImageUrl = "https://images.unsplash.com/photo-1467003909585-2f8a72700288?w=400",
                    IsPremium = false,
                    MealPlanId = wl1200.Id,
                    NutritionFacts = new NutritionFact { Calories = 450, Protein = 38, Carbs = 10, Fats = 26, Fiber = 4 },
                    MealIngredients = new List<MealIngredient>
                    {
                        new() { Ingredient = salmon, Amount = "150g" },
                        new() { Ingredient = asparagus, Amount = "100g" },
                        new() { Ingredient = oliveOil, Amount = "10g" },
                        new() { Ingredient = garlic, Amount = "2g" }
                    }
                },
                new() {
                    Name = "Greek Yogurt Cup",
                    Description = "High-protein snack",
                    mealType = MealType.Snack,
                    Difficulty = "Easy",
                    PrepTimeInMinutes = 2,
                    ImageUrl = "https://images.unsplash.com/photo-1488477181946-6428a0291777?w=400",
                    IsPremium = false,
                    MealPlanId = wl1200.Id,
                    NutritionFacts = new NutritionFact { Calories = 100, Protein = 15, Carbs = 6, Fats = 0, Fiber = 0 },
                    MealIngredients = new List<MealIngredient>
                    {
                        new() { Ingredient = greekYogurt, Amount = "150g" }
                    }
                },

                // ===== WEIGHT LOSS 1500 CAL =====
                new() {
                    Name = "Oatmeal with Berries",
                    Description = "Complex carbs with antioxidants",
                    mealType = MealType.Breakfast,
                    Difficulty = "Easy",
                    PrepTimeInMinutes = 10,
                    ImageUrl = "https://images.unsplash.com/photo-1517673400267-0251440c45cc?w=400",
                    IsPremium = false,
                    MealPlanId = wl1500.Id,
                    NutritionFacts = new NutritionFact { Calories = 350, Protein = 14, Carbs = 55, Fats = 8, Fiber = 8 },
                    MealIngredients = new List<MealIngredient>
                    {
                        new() { Ingredient = oats, Amount = "80g" },
                        new() { Ingredient = blueberries, Amount = "50g" },
                        new() { Ingredient = almondMilk, Amount = "100ml" },
                        new() { Ingredient = honey, Amount = "10g" }
                    }
                },
                new() {
                    Name = "Turkey Bento Bowl",
                    Description = "Lean turkey with rice and veggies",
                    mealType = MealType.Lunch,
                    Difficulty = "Easy",
                    PrepTimeInMinutes = 25,
                    ImageUrl = "https://images.unsplash.com/photo-1543339308-43e59d6b73a6?w=400",
                    IsPremium = false,
                    MealPlanId = wl1500.Id,
                    NutritionFacts = new NutritionFact { Calories = 550, Protein = 45, Carbs = 60, Fats = 12, Fiber = 6 },
                    MealIngredients = new List<MealIngredient>
                    {
                        new() { Ingredient = turkeyBreast, Amount = "150g" },
                        new() { Ingredient = brownRice, Amount = "100g" },
                        new() { Ingredient = broccoli, Amount = "80g" },
                        new() { Ingredient = carrots, Amount = "30g" }
                    }
                },
                new() {
                    Name = "Grilled Chicken Quinoa",
                    Description = "Complete protein with leafy greens",
                    mealType = MealType.Dinner,
                    Difficulty = "Medium",
                    PrepTimeInMinutes = 30,
                    ImageUrl = "https://images.unsplash.com/photo-1476224203421-9ac39bcb3327?w=400",
                    IsPremium = false,
                    MealPlanId = wl1500.Id,
                    NutritionFacts = new NutritionFact { Calories = 500, Protein = 42, Carbs = 45, Fats = 16, Fiber = 7 },
                    MealIngredients = new List<MealIngredient>
                    {
                        new() { Ingredient = chickenBreast, Amount = "150g" },
                        new() { Ingredient = quinoa, Amount = "80g" },
                        new() { Ingredient = spinach, Amount = "60g" },
                        new() { Ingredient = oliveOil, Amount = "10g" }
                    }
                },
                new() {
                    Name = "Apple with Almonds",
                    Description = "Fiber-rich fruit and nut combo",
                    mealType = MealType.Snack,
                    Difficulty = "Easy",
                    PrepTimeInMinutes = 1,
                    ImageUrl = "https://images.unsplash.com/photo-1560806887-1e4cd0b6cde7?w=400",
                    IsPremium = false,
                    MealPlanId = wl1500.Id,
                    NutritionFacts = new NutritionFact { Calories = 150, Protein = 4, Carbs = 18, Fats = 8, Fiber = 4 },
                    MealIngredients = new List<MealIngredient>
                    {
                        new() { Ingredient = apple, Amount = "120g" },
                        new() { Ingredient = almonds, Amount = "10g" }
                    }
                },

                // ===== WEIGHT LOSS 1800 CAL =====
                new() {
                    Name = "Protein Pancakes",
                    Description = "Fluffy protein pancakes",
                    mealType = MealType.Breakfast,
                    Difficulty = "Medium",
                    PrepTimeInMinutes = 20,
                    ImageUrl = "https://images.unsplash.com/photo-1567620905732-2d1ec7ab7445?w=400",
                    IsPremium = false,
                    MealPlanId = wl1800.Id,
                    NutritionFacts = new NutritionFact { Calories = 450, Protein = 35, Carbs = 50, Fats = 12, Fiber = 4 },
                    MealIngredients = new List<MealIngredient>
                    {
                        new() { Ingredient = oats, Amount = "60g" },
                        new() { Ingredient = wheyProtein, Amount = "30g" },
                        new() { Ingredient = eggs, Amount = "50g" },
                        new() { Ingredient = banana, Amount = "60g" },
                        new() { Ingredient = almondMilk, Amount = "100ml" }
                    }
                },
                new() {
                    Name = "Tuna Poke Bowl",
                    Description = "Protein-packed fish bowl",
                    mealType = MealType.Lunch,
                    Difficulty = "Easy",
                    PrepTimeInMinutes = 15,
                    ImageUrl = "https://images.unsplash.com/photo-1546069901-ba9599a7e63c?w=400",
                    IsPremium = false,
                    MealPlanId = wl1800.Id,
                    NutritionFacts = new NutritionFact { Calories = 650, Protein = 50, Carbs = 70, Fats = 18, Fiber = 5 },
                    MealIngredients = new List<MealIngredient>
                    {
                        new() { Ingredient = tuna, Amount = "150g" },
                        new() { Ingredient = whiteRice, Amount = "150g" },
                        new() { Ingredient = avocado, Amount = "50g" },
                        new() { Ingredient = cucumber, Amount = "50g" }
                    }
                },
                new() {
                    Name = "Steak and Sweet Potato",
                    Description = "Iron-rich dinner",
                    mealType = MealType.Dinner,
                    Difficulty = "Medium",
                    PrepTimeInMinutes = 35,
                    ImageUrl = "https://images.unsplash.com/photo-1432139555190-58524dae6a55?w=400",
                    IsPremium = true,
                    MealPlanId = wl1800.Id,
                    NutritionFacts = new NutritionFact { Calories = 600, Protein = 45, Carbs = 50, Fats = 24, Fiber = 6 },
                    MealIngredients = new List<MealIngredient>
                    {
                        new() { Ingredient = beefTenderloin, Amount = "150g" },
                        new() { Ingredient = sweetPotato, Amount = "150g" },
                        new() { Ingredient = broccoli, Amount = "80g" },
                        new() { Ingredient = oliveOil, Amount = "10g" }
                    }
                },
                new() {
                    Name = "Cottage Cheese Bowl",
                    Description = "Creamy protein snack",
                    mealType = MealType.Snack,
                    Difficulty = "Easy",
                    PrepTimeInMinutes = 2,
                    ImageUrl = "https://images.unsplash.com/photo-1559561853-08451507cbe7?w=400",
                    IsPremium = false,
                    MealPlanId = wl1800.Id,
                    NutritionFacts = new NutritionFact { Calories = 150, Protein = 18, Carbs = 8, Fats = 4, Fiber = 0 },
                    MealIngredients = new List<MealIngredient>
                    {
                        new() { Ingredient = cottageCheese, Amount = "150g" },
                        new() { Ingredient = honey, Amount = "10g" }
                    }
                },

                // ===== WEIGHT GAIN 2500 CAL =====
                new() {
                    Name = "Mega Breakfast Scramble",
                    Description = "High-calorie breakfast",
                    mealType = MealType.Breakfast,
                    Difficulty = "Easy",
                    PrepTimeInMinutes = 15,
                    ImageUrl = "https://images.unsplash.com/photo-1525351484163-7529414344d8?w=400",
                    IsPremium = false,
                    MealPlanId = gw2500.Id,
                    NutritionFacts = new NutritionFact { Calories = 700, Protein = 42, Carbs = 60, Fats = 32, Fiber = 4 },
                    MealIngredients = new List<MealIngredient>
                    {
                        new() { Ingredient = eggs, Amount = "200g" },
                        new() { Ingredient = cheese, Amount = "30g" },
                        new() { Ingredient = bacon, Amount = "30g" },
                        new() { Ingredient = wholeWheatBread, Amount = "60g" },
                        new() { Ingredient = avocado, Amount = "50g" }
                    }
                },
                new() {
                    Name = "Chicken Rice Large Bowl",
                    Description = "Mass builder bowl",
                    mealType = MealType.Lunch,
                    Difficulty = "Easy",
                    PrepTimeInMinutes = 25,
                    ImageUrl = "https://images.unsplash.com/photo-1604908176997-125f25cc6f3d?w=400",
                    IsPremium = false,
                    MealPlanId = gw2500.Id,
                    NutritionFacts = new NutritionFact { Calories = 900, Protein = 65, Carbs = 100, Fats = 28, Fiber = 5 },
                    MealIngredients = new List<MealIngredient>
                    {
                        new() { Ingredient = chickenBreast, Amount = "200g" },
                        new() { Ingredient = whiteRice, Amount = "200g" },
                        new() { Ingredient = avocado, Amount = "50g" },
                        new() { Ingredient = oliveOil, Amount = "15g" }
                    }
                },
                new() {
                    Name = "Salmon Power Dinner",
                    Description = "Omega-3 muscle recovery",
                    mealType = MealType.Dinner,
                    Difficulty = "Medium",
                    PrepTimeInMinutes = 30,
                    ImageUrl = "https://images.unsplash.com/photo-1467003909585-2f8a72700288?w=400",
                    IsPremium = false,
                    MealPlanId = gw2500.Id,
                    NutritionFacts = new NutritionFact { Calories = 750, Protein = 52, Carbs = 65, Fats = 32, Fiber = 5 },
                    MealIngredients = new List<MealIngredient>
                    {
                        new() { Ingredient = salmon, Amount = "200g" },
                        new() { Ingredient = quinoa, Amount = "120g" },
                        new() { Ingredient = sweetPotato, Amount = "100g" },
                        new() { Ingredient = oliveOil, Amount = "15g" }
                    }
                },
                new() {
                    Name = "Peanut Butter Banana Shake",
                    Description = "Mass gainer shake",
                    mealType = MealType.Snack,
                    Difficulty = "Easy",
                    PrepTimeInMinutes = 5,
                    ImageUrl = "https://images.unsplash.com/photo-1553530666-ba11a7da0696?w=400",
                    IsPremium = false,
                    MealPlanId = gw2500.Id,
                    NutritionFacts = new NutritionFact { Calories = 350, Protein = 20, Carbs = 45, Fats = 12, Fiber = 4 },
                    MealIngredients = new List<MealIngredient>
                    {
                        new() { Ingredient = wheyProtein, Amount = "30g" },
                        new() { Ingredient = banana, Amount = "100g" },
                        new() { Ingredient = peanutButter, Amount = "20g" },
                        new() { Ingredient = milk, Amount = "200ml" }
                    }
                },

                // ===== WEIGHT GAIN 3000 CAL =====
                new() {
                    Name = "Triple Protein Oatmeal",
                    Description = "Massive morning carbs",
                    mealType = MealType.Breakfast,
                    Difficulty = "Easy",
                    PrepTimeInMinutes = 10,
                    ImageUrl = "https://images.unsplash.com/photo-1517673400267-0251440c45cc?w=400",
                    IsPremium = false,
                    MealPlanId = gw3000.Id,
                    NutritionFacts = new NutritionFact { Calories = 850, Protein = 50, Carbs = 110, Fats = 22, Fiber = 8 },
                    MealIngredients = new List<MealIngredient>
                    {
                        new() { Ingredient = oats, Amount = "120g" },
                        new() { Ingredient = wheyProtein, Amount = "30g" },
                        new() { Ingredient = banana, Amount = "100g" },
                        new() { Ingredient = almonds, Amount = "20g" },
                        new() { Ingredient = milk, Amount = "200ml" }
                    }
                },
                new() {
                    Name = "Beef Large Bowl",
                    Description = "Red meat mass builder",
                    mealType = MealType.Lunch,
                    Difficulty = "Easy",
                    PrepTimeInMinutes = 30,
                    ImageUrl = "https://images.unsplash.com/photo-1543339308-43e59d6b73a6?w=400",
                    IsPremium = false,
                    MealPlanId = gw3000.Id,
                    NutritionFacts = new NutritionFact { Calories = 1100, Protein = 75, Carbs = 120, Fats = 32, Fiber = 6 },
                    MealIngredients = new List<MealIngredient>
                    {
                        new() { Ingredient = beefTenderloin, Amount = "250g" },
                        new() { Ingredient = whiteRice, Amount = "250g" },
                        new() { Ingredient = sweetPotato, Amount = "100g" },
                        new() { Ingredient = avocado, Amount = "50g" }
                    }
                },
                new() {
                    Name = "Triple Protein Dinner",
                    Description = "Ultimate recovery meal",
                    mealType = MealType.Dinner,
                    Difficulty = "Medium",
                    PrepTimeInMinutes = 40,
                    ImageUrl = "https://images.unsplash.com/photo-1432139555190-58524dae6a55?w=400",
                    IsPremium = true,
                    MealPlanId = gw3000.Id,
                    NutritionFacts = new NutritionFact { Calories = 900, Protein = 80, Carbs = 80, Fats = 35, Fiber = 5 },
                    MealIngredients = new List<MealIngredient>
                    {
                        new() { Ingredient = salmon, Amount = "200g" },
                        new() { Ingredient = chickenBreast, Amount = "150g" },
                        new() { Ingredient = quinoa, Amount = "120g" },
                        new() { Ingredient = cheese, Amount = "30g" },
                        new() { Ingredient = oliveOil, Amount = "15g" }
                    }
                },
                new() {
                    Name = "Mass Gainer Shake",
                    Description = "Calorie-dense shake",
                    mealType = MealType.Snack,
                    Difficulty = "Easy",
                    PrepTimeInMinutes = 5,
                    ImageUrl = "https://images.unsplash.com/photo-1553530666-ba11a7da0696?w=400",
                    IsPremium = false,
                    MealPlanId = gw3000.Id,
                    NutritionFacts = new NutritionFact { Calories = 450, Protein = 40, Carbs = 50, Fats = 14, Fiber = 3 },
                    MealIngredients = new List<MealIngredient>
                    {
                        new() { Ingredient = wheyProtein, Amount = "40g" },
                        new() { Ingredient = banana, Amount = "100g" },
                        new() { Ingredient = peanutButter, Amount = "30g" },
                        new() { Ingredient = milk, Amount = "250ml" }
                    }
                },

                // ===== WEIGHT GAIN 3500 CAL =====
                new() {
                    Name = "Monster Breakfast",
                    Description = "Ultimate bulk breakfast",
                    mealType = MealType.Breakfast,
                    Difficulty = "Medium",
                    PrepTimeInMinutes = 20,
                    ImageUrl = "https://images.unsplash.com/photo-1525351484163-7529414344d8?w=400",
                    IsPremium = true,
                    MealPlanId = gw3500.Id,
                    NutritionFacts = new NutritionFact { Calories = 1000, Protein = 60, Carbs = 130, Fats = 30, Fiber = 6 },
                    MealIngredients = new List<MealIngredient>
                    {
                        new() { Ingredient = eggs, Amount = "250g" },
                        new() { Ingredient = whiteRice, Amount = "150g" },
                        new() { Ingredient = wholeWheatBread, Amount = "80g" },
                        new() { Ingredient = cheese, Amount = "40g" },
                        new() { Ingredient = avocado, Amount = "50g" },
                        new() { Ingredient = bacon, Amount = "30g" }
                    }
                },
                new() {
                    Name = "Mass Monster Bowl",
                    Description = "Extreme calorie bowl",
                    mealType = MealType.Lunch,
                    Difficulty = "Easy",
                    PrepTimeInMinutes = 30,
                    ImageUrl = "https://images.unsplash.com/photo-1604908176997-125f25cc6f3d?w=400",
                    IsPremium = true,
                    MealPlanId = gw3500.Id,
                    NutritionFacts = new NutritionFact { Calories = 1300, Protein = 90, Carbs = 160, Fats = 40, Fiber = 8 },
                    MealIngredients = new List<MealIngredient>
                    {
                        new() { Ingredient = chickenBreast, Amount = "250g" },
                        new() { Ingredient = beefTenderloin, Amount = "150g" },
                        new() { Ingredient = whiteRice, Amount = "300g" },
                        new() { Ingredient = sweetPotato, Amount = "150g" },
                        new() { Ingredient = avocado, Amount = "80g" },
                        new() { Ingredient = cheese, Amount = "30g" }
                    }
                },
                new() {
                    Name = "Triple Meat Dinner",
                    Description = "Maximum protein dinner",
                    mealType = MealType.Dinner,
                    Difficulty = "Hard",
                    PrepTimeInMinutes = 45,
                    ImageUrl = "https://images.unsplash.com/photo-1432139555190-58524dae6a55?w=400",
                    IsPremium = true,
                    MealPlanId = gw3500.Id,
                    NutritionFacts = new NutritionFact { Calories = 1100, Protein = 100, Carbs = 90, Fats = 45, Fiber = 6 },
                    MealIngredients = new List<MealIngredient>
                    {
                        new() { Ingredient = salmon, Amount = "200g" },
                        new() { Ingredient = beefTenderloin, Amount = "200g" },
                        new() { Ingredient = quinoa, Amount = "150g" },
                        new() { Ingredient = sweetPotato, Amount = "150g" },
                        new() { Ingredient = oliveOil, Amount = "20g" },
                        new() { Ingredient = cheese, Amount = "30g" }
                    }
                },
                new() {
                    Name = "Mega Gainer Shake",
                    Description = "Extreme shake",
                    mealType = MealType.Snack,
                    Difficulty = "Easy",
                    PrepTimeInMinutes = 5,
                    ImageUrl = "https://images.unsplash.com/photo-1553530666-ba11a7da0696?w=400",
                    IsPremium = true,
                    MealPlanId = gw3500.Id,
                    NutritionFacts = new NutritionFact { Calories = 500, Protein = 50, Carbs = 55, Fats = 18, Fiber = 4 },
                    MealIngredients = new List<MealIngredient>
                    {
                        new() { Ingredient = wheyProtein, Amount = "50g" },
                        new() { Ingredient = banana, Amount = "120g" },
                        new() { Ingredient = peanutButter, Amount = "40g" },
                        new() { Ingredient = oats, Amount = "30g" },
                        new() { Ingredient = milk, Amount = "300ml" }
                    }
                },

                // ===== GET FITTER 2000 CAL =====
                new() {
                    Name = "Balanced Breakfast",
                    Description = "Perfect morning fuel",
                    mealType = MealType.Breakfast,
                    Difficulty = "Easy",
                    PrepTimeInMinutes = 10,
                    ImageUrl = "https://images.unsplash.com/photo-1517673400267-0251440c45cc?w=400",
                    IsPremium = false,
                    MealPlanId = fit2000.Id,
                    NutritionFacts = new NutritionFact { Calories = 450, Protein = 28, Carbs = 55, Fats = 14, Fiber = 6 },
                    MealIngredients = new List<MealIngredient>
                    {
                        new() { Ingredient = oats, Amount = "80g" },
                        new() { Ingredient = eggs, Amount = "100g" },
                        new() { Ingredient = banana, Amount = "80g" },
                        new() { Ingredient = almonds, Amount = "15g" }
                    }
                },
                new() {
                    Name = "Balanced Lunch Bowl",
                    Description = "Complete macro meal",
                    mealType = MealType.Lunch,
                    Difficulty = "Easy",
                    PrepTimeInMinutes = 20,
                    ImageUrl = "https://images.unsplash.com/photo-1543339308-43e59d6b73a6?w=400",
                    IsPremium = false,
                    MealPlanId = fit2000.Id,
                    NutritionFacts = new NutritionFact { Calories = 600, Protein = 45, Carbs = 65, Fats = 18, Fiber = 6 },
                    MealIngredients = new List<MealIngredient>
                    {
                        new() { Ingredient = chickenBreast, Amount = "150g" },
                        new() { Ingredient = brownRice, Amount = "120g" },
                        new() { Ingredient = broccoli, Amount = "80g" },
                        new() { Ingredient = avocado, Amount = "30g" }
                    }
                },
                new() {
                    Name = "Salmon with Quinoa",
                    Description = "Omega-3 balanced dinner",
                    mealType = MealType.Dinner,
                    Difficulty = "Medium",
                    PrepTimeInMinutes = 25,
                    ImageUrl = "https://images.unsplash.com/photo-1476224203421-9ac39bcb3327?w=400",
                    IsPremium = false,
                    MealPlanId = fit2000.Id,
                    NutritionFacts = new NutritionFact { Calories = 650, Protein = 45, Carbs = 55, Fats = 28, Fiber = 6 },
                    MealIngredients = new List<MealIngredient>
                    {
                        new() { Ingredient = salmon, Amount = "180g" },
                        new() { Ingredient = quinoa, Amount = "100g" },
                        new() { Ingredient = spinach, Amount = "60g" },
                        new() { Ingredient = oliveOil, Amount = "12g" }
                    }
                },
                new() {
                    Name = "Greek Yogurt Parfait",
                    Description = "Protein-packed snack",
                    mealType = MealType.Snack,
                    Difficulty = "Easy",
                    PrepTimeInMinutes = 5,
                    ImageUrl = "https://images.unsplash.com/photo-1488477181946-6428a0291777?w=400",
                    IsPremium = false,
                    MealPlanId = fit2000.Id,
                    NutritionFacts = new NutritionFact { Calories = 250, Protein = 20, Carbs = 25, Fats = 8, Fiber = 3 },
                    MealIngredients = new List<MealIngredient>
                    {
                        new() { Ingredient = greekYogurt, Amount = "180g" },
                        new() { Ingredient = blueberries, Amount = "50g" },
                        new() { Ingredient = almonds, Amount = "10g" }
                    }
                },

                // ===== GET FITTER 2500 CAL =====
                new() {
                    Name = "Power Breakfast",
                    Description = "High-energy morning",
                    mealType = MealType.Breakfast,
                    Difficulty = "Easy",
                    PrepTimeInMinutes = 15,
                    ImageUrl = "https://images.unsplash.com/photo-1525351484163-7529414344d8?w=400",
                    IsPremium = false,
                    MealPlanId = fit2500.Id,
                    NutritionFacts = new NutritionFact { Calories = 600, Protein = 40, Carbs = 70, Fats = 18, Fiber = 6 },
                    MealIngredients = new List<MealIngredient>
                    {
                        new() { Ingredient = eggs, Amount = "150g" },
                        new() { Ingredient = wholeWheatBread, Amount = "80g" },
                        new() { Ingredient = avocado, Amount = "50g" },
                        new() { Ingredient = banana, Amount = "80g" }
                    }
                },
                new() {
                    Name = "Power Lunch Bowl",
                    Description = "Training day fuel",
                    mealType = MealType.Lunch,
                    Difficulty = "Easy",
                    PrepTimeInMinutes = 25,
                    ImageUrl = "https://images.unsplash.com/photo-1604908176997-125f25cc6f3d?w=400",
                    IsPremium = false,
                    MealPlanId = fit2500.Id,
                    NutritionFacts = new NutritionFact { Calories = 800, Protein = 60, Carbs = 90, Fats = 24, Fiber = 8 },
                    MealIngredients = new List<MealIngredient>
                    {
                        new() { Ingredient = turkeyBreast, Amount = "200g" },
                        new() { Ingredient = whiteRice, Amount = "180g" },
                        new() { Ingredient = sweetPotato, Amount = "100g" },
                        new() { Ingredient = broccoli, Amount = "80g" },
                        new() { Ingredient = avocado, Amount = "40g" }
                    }
                },
                new() {
                    Name = "Recovery Dinner",
                    Description = "Post-workout recovery",
                    mealType = MealType.Dinner,
                    Difficulty = "Medium",
                    PrepTimeInMinutes = 30,
                    ImageUrl = "https://images.unsplash.com/photo-1467003909585-2f8a72700288?w=400",
                    IsPremium = false,
                    MealPlanId = fit2500.Id,
                    NutritionFacts = new NutritionFact { Calories = 850, Protein = 60, Carbs = 85, Fats = 32, Fiber = 7 },
                    MealIngredients = new List<MealIngredient>
                    {
                        new() { Ingredient = salmon, Amount = "200g" },
                        new() { Ingredient = quinoa, Amount = "150g" },
                        new() { Ingredient = spinach, Amount = "80g" },
                        new() { Ingredient = sweetPotato, Amount = "100g" },
                        new() { Ingredient = oliveOil, Amount = "15g" }
                    }
                },
                new() {
                    Name = "Post-Workout Shake",
                    Description = "Recovery shake",
                    mealType = MealType.Snack,
                    Difficulty = "Easy",
                    PrepTimeInMinutes = 5,
                    ImageUrl = "https://images.unsplash.com/photo-1553530666-ba11a7da0696?w=400",
                    IsPremium = false,
                    MealPlanId = fit2500.Id,
                    NutritionFacts = new NutritionFact { Calories = 350, Protein = 35, Carbs = 40, Fats = 10, Fiber = 3 },
                    MealIngredients = new List<MealIngredient>
                    {
                        new() { Ingredient = wheyProtein, Amount = "35g" },
                        new() { Ingredient = banana, Amount = "100g" },
                        new() { Ingredient = almondMilk, Amount = "200ml" }
                    }
                }
            };

            await ctx.meals.AddRangeAsync(meals);
            await ctx.SaveChangesAsync();
        }
    }
}
