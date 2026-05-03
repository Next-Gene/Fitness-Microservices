using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NutritionService.Domain.Models;
using NutritionService.Domain.Models.Enums;
using NutritionService.Features.Meals.GetMealDetails;
using NutritionService.Infrastructure.Data;
using Xunit;

namespace NutritionService.Tests.Features.Meals.GetMealDetails
{
    public class GetMealDetailsHandlerTests
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMemoryCache _cache;
        private readonly GetMealDetailsHandler _handler;

        public GetMealDetailsHandlerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _cache = new MemoryCache(new MemoryCacheOptions());
            _handler = new GetMealDetailsHandler(_dbContext, _cache);
        }

        [Fact]
        public async Task Handle_MealExists_ReturnsMealDetails()
        {
            // Arrange
            var mealId = 1;
            var meal = new Meal
            {
                Id = mealId,
                Name = "Test Meal",
                Description = "Test Description",
                mealType = MealType.Breakfast,
                PrepTimeInMinutes = 20,
                Difficulty = "Easy",
                ImageUrl = "http://example.com/image.jpg",
                IsPremium = false,
                IsDeleted = false,
                NutritionFacts = new NutritionFact
                {
                    Calories = 500,
                    Protein = 30,
                    Carbs = 50,
                    Fats = 20,
                    Fiber = 10
                },
                MealIngredients = new List<MealIngredient>
                {
                    new MealIngredient
                    {
                        Ingredient = new Ingredient { Name = "Egg" },
                        Amount = "2"
                    }
                }
            };

            await _dbContext.Meals.AddAsync(meal);
            await _dbContext.SaveChangesAsync();

            var query = new GetMealDetailsQuery { Id = mealId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Id.Should().Be(mealId);
            result.Data.Name.Should().Be("Test Meal");
            result.Data.Nutrition.Calories.Should().Be(500);
            result.Data.Ingredients.Should().HaveCount(1);
            result.Data.Ingredients[0].Name.Should().Be("Egg");
        }

        [Fact]
        public async Task Handle_MealDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var query = new GetMealDetailsQuery { Id = 999 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Meal not found");
        }

        [Fact]
        public async Task Handle_MealIsCached_ReturnsFromCache()
        {
            // Arrange
            var mealId = 1;
            var meal = new Meal
            {
                Id = mealId,
                Name = "Cached Meal",
                Description = "Test",
                mealType = MealType.Lunch,
                PrepTimeInMinutes = 30,
                Difficulty = "Medium",
                ImageUrl = "test.jpg",
                NutritionFacts = new NutritionFact { Calories = 400 }
            };

            await _dbContext.Meals.AddAsync(meal);
            await _dbContext.SaveChangesAsync();

            var query = new GetMealDetailsQuery { Id = mealId };

            // First call to populate cache
            await _handler.Handle(query, CancellationToken.None);

            // Modify DB to check if it's still returning from cache
            meal.Name = "Modified Meal";
            _dbContext.Meals.Update(meal);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Data.Name.Should().Be("Cached Meal");
            result.Message.Should().Contain("from cache");
        }
    }
}
