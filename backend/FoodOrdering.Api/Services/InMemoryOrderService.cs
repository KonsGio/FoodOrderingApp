using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FoodOrdering.Api.Models;

namespace FoodOrdering.Api.Services;

public sealed class InMemoryOrderService : IOrderService
{
    private readonly IMenuRepository _menuRepository;

    public InMemoryOrderService(IMenuRepository menuRepository)
    {
        _menuRepository = menuRepository;
    }

    public async Task<OrderResponse> PlaceOrderAsync(OrderRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Items is null || request.Items.Count == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(request.Items), "Order must contain at least one item.");
        }

        // Get full menu once
        var menu = await _menuRepository.GetMenuAsync(cancellationToken);
        var mealsById = menu.ToDictionary(m => m.Id);

        var summaries = new List<OrderItemSummary>();
        decimal total = 0;

        foreach (var item in request.Items)
        {
            if (item.Quantity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(item.Quantity), "Quantity must be greater than zero.");
            }

            if (!mealsById.TryGetValue(item.MealId, out var meal))
            {
                throw new InvalidOperationException($"Meal with id {item.MealId} was not found.");
            }

            var lineTotal = meal.Price * item.Quantity;
            total += lineTotal;

            summaries.Add(new OrderItemSummary
            {
                MealId = item.MealId,
                Quantity = item.Quantity,
                LineTotal = lineTotal
            });
        }

        return new OrderResponse
        {
            OrderId = Guid.NewGuid(),
            TotalPrice = total,
            CreatedAtUtc = DateTime.UtcNow,
            Items = summaries
        };
    }
}