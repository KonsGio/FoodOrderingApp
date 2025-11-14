using System;
using System.Collections.Generic;

namespace FoodOrdering.Api.Models;

public sealed class OrderItemSummary
{
    public int MealId { get; init; }
    public int Quantity { get; init; }
    public decimal LineTotal { get; init; }
}

public sealed class OrderResponse
{
    public Guid OrderId { get; init; }
    public decimal TotalPrice { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public IReadOnlyCollection<OrderItemSummary> Items { get; init; } = Array.Empty<OrderItemSummary>();
}