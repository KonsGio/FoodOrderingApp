using System;
using System.Collections.Generic;

namespace FoodOrdering.Api.Models;

public sealed class OrderRequest
{
    public IReadOnlyCollection<OrderItemRequest> Items { get; init; } = Array.Empty<OrderItemRequest>();
}