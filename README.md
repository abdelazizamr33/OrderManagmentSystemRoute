# OrderManagmentSystem

A modular, extensible ASP.NET Core Web API for managing orders, products, customers, and invoices. Built with N-Tier architecture principles, Entity Framework Core, and AutoMapper.

---

## Features

- **User Registration & Authentication** (JWT-based)
- **Product Management** (CRUD, stock updates)
- **Order Management** (create, view, update status)
- **Customer Management** (create, view orders)
- **Invoice Generation**
- **Role-based Authorization** (Admin, Customer)
- **Swagger/OpenAPI** for easy API exploration

---

## Technologies

- ASP.NET Core 8
- Entity Framework Core (SQL Server)
- AutoMapper
- Swashbuckle (Swagger)
- Dependency Injection
- Layered Architecture (API, BLL, DAL)

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server (local or remote)

### Build & Run

```bash
dotnet restore
dotnet build
dotnet ef database update   # Apply migrations (if any)
dotnet run --project OrderManagmentSystem/OrderManagmentSystem.csproj
```

Then visit:
[https://localhost:7013/swagger](https://localhost:7013/swagger) — interactive API docs

---

## API Overview

### User Endpoints

* `POST /api/user/register` — Register a new user
* `POST /api/user/login` — Login and receive JWT

### Product Endpoints

* `GET /api/product` — List all products
* `GET /api/product/{productId}` — Get product by ID
* `POST /api/product` — Add product (Admin only)
* `PUT /api/product/{productId}` — Update product (Admin only)

### Order Endpoints

* `POST /api/order?customerId={id}` — Create order (Customer/Admin)
* `GET /api/order/{orderId}` — Get order by ID (Customer/Admin)
* `GET /api/order` — List all orders (Admin only)
* `PUT /api/order/{orderId}/status` — Update order status (Admin only)

### Customer Endpoints

* `POST /api/customer` — Create customer
* `GET /api/customer/{customerId}/orders` — Get orders for a customer

### Invoice Endpoints

* `GET /api/invoice/{invoiceId}` — Get invoice by ID (Admin only)
* `GET /api/invoice` — List all invoices (Admin only)

---

## Domain Models

* **User**: Authentication and role management.
* **Customer**: Linked to User, holds customer info.
* **Product**: Name, price, stock.
* **Order**: Customer, date, total, status, payment method, items.
* **OrderItem**: Product, quantity, price.
* **Invoice**: Linked to Order, total, date.

> All navigation collections are initialized to prevent null reference exceptions.

---

## Development Notes

* AutoMapper handles all DTO ↔ Entity conversions.
* All business logic is abstracted into BLL services.
* Clean architecture with strict separation between API, business logic, and data access.
* Fully dependency-injected, testable structure.

---


