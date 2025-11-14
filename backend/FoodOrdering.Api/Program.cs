using FoodOrdering.Api.Models;
using FoodOrdering.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// CORS for Next.js dev on localhost:3000
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// DI
builder.Services.AddSingleton<IMenuRepository, InMemoryMenuRepository>();
builder.Services.AddScoped<IOrderService, InMemoryOrderService>();

var app = builder.Build();

app.UseCors();

// Menu endpoint CDN
app.MapGet("/api/menu", async (IMenuRepository menuRepository, HttpContext httpContext, CancellationToken ct) =>
{
    httpContext.Response.Headers.CacheControl = "public,max-age=60";

    var meals = await menuRepository.GetMenuAsync(ct);
    return Results.Ok(meals);
})
.WithName("GetMenu");

// Order endpoint
app.MapPost("/api/orders", async (OrderRequest request, IOrderService orderService, CancellationToken ct) =>
{
    try
    {
        var response = await orderService.PlaceOrderAsync(request, ct);
        return Results.Created($"/api/orders/{response.OrderId}", response);
    }
    catch (ArgumentOutOfRangeException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
})
.WithName("CreateOrder");

app.Run();