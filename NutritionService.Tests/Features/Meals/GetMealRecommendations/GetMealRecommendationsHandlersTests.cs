using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NutritionService.Domain.Models;
using NutritionService.Domain.Models.Enums;
using NutritionService.Features.Meals.GetMealRecommendations;
using NutritionService.Infrastructure.Data;
using Xunit;

namespace NutritionService.Tests.Features.Meals.GetMealRecommendations
{
    public class GetMealRecommendationsHandlersTests
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMemoryCache _cache;
        private readonly GetMealRecommendationsHandlers _handler;

        public GetMealRecommendationsHandlersTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _cache = new MemoryCache(new MemoryCacheOptions());
            _handler = new GetMealRecommendationsHandlers(_dbContext, _cache);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsPaginatedRecommendations()
        {
            // Arrange
            var meals = new List<Meal>
            {
                new Meal { Id = 1, Name = "Breakfast Meal", mealType = MealType.Breakfast, NutritionFacts = new NutritionFact { Calories = 300, Protein = 20 }, Description = "Test", ImageUrl = "test.jpg", Difficulty = "Easy" },
                new Meal { Id = 2, Name = "Lunch Meal", mealType = MealType.Lunch, NutritionFacts = new NutritionFact { Calories = 500, Protein = 30 }, Description = "Test", ImageUrl = "test.jpg", Difficulty = "Medium" },
                new Meal { Id = 3, Name = "Another Breakfast", mealType = MealType.Breakfast, NutritionFacts = new NutritionFact { Calories = 350, Protein = 15 }, Description = "Test", ImageUrl = "test.jpg", Difficulty = "Easy" }
            };

            await _dbContext.Meals.AddRangeAsync(meals);
            await _dbContext.SaveChangesAsync();

            var query = new GetMealRecommendationsQuery 
            { 
                Page = 1, 
                PageSize = 10,
                MealType = "Breakfast"
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(2);
            result.Items.Should().OnlyContain(m => m.MealType == "Breakfast");
            result.TotalCount.Should().Be(2);
        }

        [Fact]
        public async Task Handle_CaloriesFilter_FiltersCorrectly()
        {
            // Arrange
            var meals = new List<Meal>
            {
                new Meal { Id = 4, Name = "Low Calorie", mealType = MealType.Dinner, NutritionFacts = new NutritionFact { Calories = 200, Protein = 10 }, Description = "Test", ImageUrl = "test.jpg", Difficulty = "Easy" },
                new Meal { Id = 5, Name = "High Calorie", mealType = MealType.Dinner, NutritionFacts = new NutritionFact { Calories = 800, Protein = 40 }, Description = "Test", ImageUrl = "test.jpg", Difficulty = "Hard" }
            };

            await _dbContext.Meals.AddRangeAsync(meals);
            await _dbContext.SaveChangesAsync();

            var query = new GetMealRecommendationsQuery 
            { 
                Page = 1, 
                PageSize = 10,
                MaxCalories = 300
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Items.Should().HaveCount(1);
            result.Items[0].Name.Should().Be("Low Calorie");
        }
    }
}
