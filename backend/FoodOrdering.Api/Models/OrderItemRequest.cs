namespace FoodOrdering.Api.Models;

public sealed class OrderItemRequest
{
    public int MealId { get; init; }
    public int Quantity { get; init; }
}