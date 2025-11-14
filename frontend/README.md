# Food Ordering Frontend (Next.js)

A minimal, clean React/Next.js UI for browsing meals and placing orders.

---

## Features

- Displays menu from backend
- Clean and scrollable UI
- Increment/decrement quantities per meal
- Order selected meals in one request
- Light, responsive design

---

## API Setup

Create `.env.local`:

```
NEXT_PUBLIC_API_BASE_URL=http://localhost:5000
```

---

## Run the Frontend

```bash
cd frontend
npm install
npm run dev
```

Runs at:

```
http://localhost:3000
```

---

## Architecture Notes

- One fetch for menu
- Client-side state for quantity updates
- One POST call for order submission
- Works with any backend implementing the same API contract
