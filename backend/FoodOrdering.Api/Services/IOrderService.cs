using FoodOrdering.Api.Models;

namespace FoodOrdering.Api.Services;

public interface IOrderService
{
    Task<OrderResponse> PlaceOrderAsync(OrderRequest request, CancellationToken cancellationToken = default);
}