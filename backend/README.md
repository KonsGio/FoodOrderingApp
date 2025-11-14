# Food Ordering API (Backend)

A lightweight .NET 8 API that exposes a menu and processes food orders.  
Designed with scalability and clean architecture in mind.

---

## Features

- .NET 8 minimal API
- In-memory storage (abstracted for easy replacement later)
- CDN‑friendly menu endpoint
- Supports ordering **one or many meals at once**
- Clean architecture using interfaces (`IMenuRepository`, `IOrderService`)
- Unit tests included
- CORS enabled for the frontend

---

## Endpoints

### **GET /api/menu**
Returns a list of meals.  
Includes HTTP caching:

```
Cache-Control: public, max-age=60
```

Ready for CDN-heavy workloads.

---

### **POST /api/orders**
Accepts an order such as:

```json
{
  "items": [
    { "mealId": 1, "quantity": 2 },
    { "mealId": 4, "quantity": 1 }
  ]
}
```

Returns an order summary with totals.

---

## Architecture & Scalability

- **Menu** endpoint is read-heavy and cacheable.
- **Order** endpoint is stateless and horizontally scalable.
- In-memory storage is abstracted so future DB integration requires zero endpoint changes.
- Backend is safe to run behind a load balancer.

---

## Running the Backend

```bash
cd backend/FoodOrdering.Api
dotnet restore
dotnet run
```

Runs at:

```
http://localhost:5000
```

---

## Running Unit Tests

```bash
cd backend/FoodOrdering.Api/tests/FoodOrdering.Tests
dotnet test
```
