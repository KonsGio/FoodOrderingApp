import type {
    Meal,
    OrderRequest,
    OrderResponse,
} from "@/types";

const API_BASE_URL =
    process.env.NEXT_PUBLIC_API_BASE_URL;

export async function fetchMenu(): Promise<Meal[]> {
    const res = await fetch(`${API_BASE_URL}/api/menu`, { cache: "no-store" });
    if (!res.ok) {
        throw new Error("Failed to fetch menu");
    }
    return res.json();
}

export async function placeOrder(request: OrderRequest): Promise<OrderResponse> {
    const res = await fetch(`${API_BASE_URL}/api/orders`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(request),
    });

    if (!res.ok) {
        const errorPayload = await res.json().catch(() => ({}));
        throw new Error(errorPayload.error ?? "Failed to place order");
    }

    return res.json();
}