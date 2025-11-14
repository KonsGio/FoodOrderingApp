"use client";

import {useEffect, useMemo, useState} from "react";
import type {Meal, OrderItemRequest} from "@/types";
import {fetchMenu, placeOrder} from "@/lib/api";

type Quantities = Record<number, number>;

export default function HomePage() {
    const [meals, setMeals] = useState<Meal[]>([]);
    const [quantities, setQuantities] = useState<Quantities>({});
    const [isLoadingMenu, setIsLoadingMenu] = useState(true);
    const [isOrdering, setIsOrdering] = useState(false);
    const [message, setMessage] = useState<string | null>(null);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const loadMenu = async () => {
            try {
                const data = await fetchMenu();
                setMeals(data);
            } catch (e) {
                console.error(e);
                setError("Could not load the menu. Please try again.");
            } finally {
                setIsLoadingMenu(false);
            }
        };

        loadMenu();
    }, []);

    const increment = (mealId: number) => {
        setQuantities((prev) => ({
            ...prev, [mealId]: (prev[mealId] ?? 0) + 1,
        }));
    };

    const decrement = (mealId: number) => {
        setQuantities((prev) => {
            const current = prev[mealId] ?? 0;
            const next = Math.max(0, current - 1);
            return {...prev, [mealId]: next};
        });
    };

    const selection = useMemo(() => {
        const items: OrderItemRequest[] = [];
        let totalEstimated = 0;
        let totalQuantity = 0;

        for (const meal of meals) {
            const quantity = quantities[meal.id] ?? 0;
            if (quantity > 0) {
                items.push({mealId: meal.id, quantity});
                totalEstimated += meal.price * quantity;
                totalQuantity += quantity;
            }
        }

        return {items, totalEstimated, totalQuantity};
    }, [meals, quantities]);

    const hasSelection = selection.items.length > 0;

    const handleOrderAll = async () => {
        if (!hasSelection) {
            setError("Please select at least one meal before ordering.");
            setMessage(null);
            return;
        }

        setError(null);
        setMessage(null);
        setIsOrdering(true);

        try {
            const response = await placeOrder({items: selection.items});
            setMessage(`Order ${response.orderId} placed: ${selection.totalQuantity} item(s), total €${response.totalPrice.toFixed(2)}`);
            setQuantities({});
        } catch (e: any) {
            setError(e.message ?? "Failed to place order.");
        } finally {
            setIsOrdering(false);
        }
    };

    if (isLoadingMenu) {
        return (<main className="min-h-screen flex items-center justify-center bg-slate-50">
            <div className="rounded-xl border border-slate-200 bg-white px-6 py-4 shadow-sm">
                <p className="text-base font-medium text-slate-800">Loading menu…</p>
            </div>
        </main>);
    }

    return (<main className="min-h-screen bg-slate-50">
        <header className="border-b border-slate-200 bg-white">
            <div className="mx-auto flex max-w-3xl items-center justify-between px-4 py-3">
                <h1 className="text-lg font-semibold text-slate-900">Place your order</h1>
            </div>
        </header>

        <div className="mx-auto max-w-3xl px-4 py-6 space-y-4">
            <p className="text-sm text-slate-600">
                Select one or more meals, adjust the quantity with the plus/minus buttons, then press{" "}
                <span className="font-medium">Order selected meals</span>.
            </p>

            {error && (<div className="rounded-md border border-red-200 bg-red-50 px-3 py-2 text-sm text-red-700">
                {error}
            </div>)}

            {message && (<div
                className="rounded-md border border-emerald-200 bg-emerald-50 px-3 py-2 text-sm text-emerald-800">
                {message}
            </div>)}

            {meals.length === 0 ? (<p className="text-sm text-slate-600">No meals available right now.</p>) : (<>
                <div className="max-h-[500px] overflow-y-auto pr-1">
                    <div className="grid gap-4 sm:grid-cols-2">
                        {meals.map((meal) => {
                            const quantity = quantities[meal.id] ?? 0;

                            return (<article
                                key={meal.id}
                                className="flex flex-col justify-between rounded-xl border border-slate-200 bg-white p-4 shadow-sm"
                            >
                                <div>
                                    <h2 className="text-base font-semibold text-slate-900">{meal.name}</h2>
                                    <p className="mt-1 text-xs text-slate-600">{meal.description}</p>
                                    <p className="mt-3 text-sm font-medium text-slate-900">
                                        €{meal.price.toFixed(2)}
                                        {quantity > 0 && (<span className="ml-2 text-xs text-slate-500">
                              · {quantity} selected (- €
                                            {(meal.price * quantity).toFixed(2)})
                            </span>)}
                                    </p>
                                </div>

                                <div className="mt-4 flex items-center justify-between">
                                    <div className="flex items-center space-x-2">
                                        <button
                                            type="button"
                                            onClick={() => decrement(meal.id)}
                                            className="h-8 w-8 rounded-full border border-slate-300 bg-white text-lg leading-none text-slate-700 hover:bg-slate-100"
                                            aria-label={`Decrease quantity for ${meal.name}`}
                                        >
                                            −
                                        </button>
                                        <span
                                            className="w-8 text-center text-sm tabular-nums text-slate-900">
                            {quantity}
                          </span>
                                        <button
                                            type="button"
                                            onClick={() => increment(meal.id)}
                                            className="h-8 w-8 rounded-full border border-slate-300 bg-white text-lg leading-none text-slate-700 hover:bg-slate-100"
                                            aria-label={`Increase quantity for ${meal.name}`}
                                        >
                                            +
                                        </button>
                                    </div>
                                </div>
                            </article>);
                        })}
                    </div>
                </div>

                <div
                    className="flex items-center justify-between rounded-xl border border-slate-200 bg-white px-4 py-3 shadow-sm">
                    <div className="text-sm text-slate-700">
                        <p className="font-medium">
                            Selected items:{" "}
                            <span className="font-semibold">
                    {selection.totalQuantity}
                  </span>
                        </p>
                        <p className="text-xs text-slate-500">
                            Estimated total: €{selection.totalEstimated.toFixed(2)}
                        </p>
                    </div>
                    <button type="button"
                            onClick={handleOrderAll}
                            disabled={!hasSelection || isOrdering}
                            className="rounded-full bg-emerald-500 px-4 py-2 text-xs font-semibold text-white shadow-sm hover:bg-emerald-600 disabled:opacity-60 disabled:hover:bg-emerald-500"
                    >
                        {isOrdering ? "Ordering…" : "Order selected meals"}
                    </button>
                </div>
            </>)}
        </div>
    </main>);
}