using System;

namespace FoodOrdering.Api.Models
{
    public sealed class Meal
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public decimal Price { get; init; }
    }
}