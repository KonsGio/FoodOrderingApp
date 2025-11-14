using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FoodOrdering.Api.Models;
using FoodOrdering.Api.Services;
using Xunit;

namespace FoodOrdering.Tests;

public class OrderServiceTests
{
    private sealed class FakeMenuRepository : IMenuRepository
    {
        private readonly IReadOnlyCollection<Meal> _meals;

        public FakeMenuRepository(IReadOnlyCollection<Meal> meals)
        {
            _meals = meals;
        }

        public Task<IReadOnlyCollection<Meal>> GetMenuAsync(CancellationToken cancellationToken = default)
            => Task.FromResult(_meals);

        public Task<Meal?> GetMealByIdAsync(int mealId, CancellationToken cancellationToken = default)
        {
            Meal? meal = null;
            foreach (var m in _meals)
            {
                if (m.Id == mealId)
                {
                    meal = m;
                    break;
                }
            }

            return Task.FromResult(meal);
        }
    }

    private static InMemoryOrderService CreateServiceWithMeals(params Meal[] meals)
    {
        var repo = new FakeMenuRepository(meals);
        return new InMemoryOrderService(repo);
    }

    [Fact]
    public async Task PlaceOrderAsync_SingleItem_ComputesTotal()
    {
        // Arrange
        var service = CreateServiceWithMeals(
            new Meal { Id = 1, Name = "Test 1", Description = "Test", Price = 10m }
        );

        var request = new OrderRequest
        {
            Items = new[]
            {
                new OrderItemRequest { MealId = 1, Quantity = 2 }
            }
        };

        // Act
        var result = await service.PlaceOrderAsync(request);

        // Assert
        Assert.Equal(20m, result.TotalPrice);
        Assert.Single(result.Items);
    }

    [Fact]
    public async Task PlaceOrderAsync_MultipleItems_SumsTotals()
    {
        var service = CreateServiceWithMeals(
            new Meal { Id = 1, Name = "Test 1", Description = "Test", Price = 10m },
            new Meal { Id = 2, Name = "Test 2", Description = "Test", Price = 5m }
        );

        var request = new OrderRequest
        {
            Items = new[]
            {
                new OrderItemRequest { MealId = 1, Quantity = 2 },
                new OrderItemRequest { MealId = 2, Quantity = 3 }
            }
        };

        var result = await service.PlaceOrderAsync(request);

        Assert.Equal(35m, result.TotalPrice);
        Assert.Equal(2, result.Items.Count);
    }
}