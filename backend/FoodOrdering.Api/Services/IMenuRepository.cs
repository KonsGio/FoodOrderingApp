using FoodOrdering.Api.Models;

namespace FoodOrdering.Api.Services;

public interface IMenuRepository
{
    Task<IReadOnlyCollection<Meal>> GetMenuAsync(CancellationToken cancellationToken = default);
    Task<Meal?> GetMealByIdAsync(int mealId, CancellationToken cancellationToken = default);
}