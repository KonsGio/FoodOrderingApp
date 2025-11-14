export interface Meal {
    id: number;
    name: string;
    description: string;
    price: number;
}

export interface OrderItemRequest {
    mealId: number;
    quantity: number;
}

export interface OrderRequest {
    items: OrderItemRequest[];
}

export interface OrderItemSummary {
    mealId: number;
    quantity: number;
    lineTotal: number;
}

export interface OrderResponse {
    orderId: string;
    totalPrice: number;
    createdAtUtc: string;
    items: OrderItemSummary[];
}