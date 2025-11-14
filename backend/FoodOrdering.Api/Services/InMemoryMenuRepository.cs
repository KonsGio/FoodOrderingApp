using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FoodOrdering.Api.Models;

namespace FoodOrdering.Api.Services
{
    public sealed class InMemoryMenuRepository : IMenuRepository
    {
        private static readonly IReadOnlyList<Meal> _meals = new List<Meal>
        {
            new Meal { Id = 1, Name = "Margherita Pizza", Description = "Tomato, mozzarella, basil", Price = 9.50m },
            new Meal { Id = 2, Name = "Spaghetti Bolognese", Description = "Slow-cooked beef ragu", Price = 12.00m },
            new Meal { Id = 3, Name = "Caesar Salad", Description = "Romaine, parmesan, croutons", Price = 8.00m },
            new Meal { Id = 4, Name = "Grilled Chicken Bowl", Description = "Chicken, rice, veggies, lemon dressing", Price = 11.00m },
            new Meal { Id = 5, Name = "Veggie Burger", Description = "Plant-based patty, lettuce, tomato, fries", Price = 10.50m },
            new Meal { Id = 6, Name = "Sushi Platter", Description = "Mix of nigiri and maki rolls", Price = 15.00m },
            new Meal { Id = 7, Name = "Tomato Soup", Description = "Creamy tomato soup with herbs", Price = 6.50m },
            new Meal { Id = 8, Name = "Penne Alfredo", Description = "Pasta in a creamy parmesan sauce", Price = 11.50m },
            new Meal { Id = 9, Name = "Beef Tacos", Description = "Soft tortillas, spiced beef, salsa", Price = 9.00m },
            new Meal { Id = 10, Name = "Greek Salad", Description = "Feta, olives, cucumber, tomato", Price = 8.50m }
        };

        public Task<IReadOnlyCollection<Meal>> GetMenuAsync(CancellationToken cancellationToken = default)
            => Task.FromResult<IReadOnlyCollection<Meal>>(_meals);

        public Task<Meal?> GetMealByIdAsync(int mealId, CancellationToken cancellationToken = default)
            => Task.FromResult(_meals.FirstOrDefault(m => m.Id == mealId));
    }
}