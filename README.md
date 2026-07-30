md_content = """# 🛒 E-Commerce RESTful API

<div align="center">
  <h3>An Enterprise-Level ASP.NET Core Web API Project</h3>
  <p><em>Built with Clean Architecture (Onion Architecture) principles.</em></p>
  <p>Developed by <b>Moaaz Atef</b></p>
</div>

---

## 📌 Project Overview
A robust, scalable, and fully-featured RESTful API for an E-Commerce platform. The project is designed using **Clean Architecture (Onion Architecture)**, separating concerns across Domain, Application, Infrastructure, and API layers to ensure maximum maintainability and testability.

---

## 🧅 Architecture Breakdown
The solution is structured into 4 distinct layers ensuring dependencies flow **inward only**:

1. **Domain Layer**: Core Entities (`Product`, `Order`, `Basket`) and Contracts (`IGenericRepository`, `IUnitOfWork`). Depends on nothing.
2. **Application Layer**: Business logic, Service Interfaces, DTOs, and AutoMapper profiles.
3. **Infrastructure Layer**: EF Core `DbContext`, Data Seeding, Identity, and external services (Stripe, Redis).
4. **Presentation Layer (API)**: Controllers, Middleware, JWT Authentication, and Composition Root (DI).

### 🛠️ Key Design Patterns
* **Generic Repository & Unit of Work**: Ensures atomic database transactions and reusable data-access logic.
* **Specification Pattern**: Encapsulates query logic (filtering, sorting, pagination, and eager loading of related data) to keep repositories clean and flexible.
* **Data Transfer Objects (DTOs)**: Flattens complex domain models for clean, tailored API responses.

---

## 📦 Core Modules

### 1. Product Module (Catalog)
Handles the core product catalog, brands, and types using SQL Server.
* **Features**: Filtering, Searching, dynamic Sorting (Name/Price), and Pagination.
* **Picture URL Resolver**: Uses AutoMapper to dynamically construct full image URLs based on the hosting environment.

### 2. Basket Module (Shopping Cart)
Utilizes **Redis** (an in-memory NoSQL data store) for high-speed, temporary shopping cart operations.
* **Features**: Add/remove items, update quantities.
* **Time-to-Live (TTL)**: Automatically expires abandoned carts to free up server memory.

### 3. Identity Module (Authentication & Security)
Built on **ASP.NET Core Identity** and **JWT (JSON Web Tokens)** for stateless authentication.
* **Features**: User registration, secure login, role-based authorization, and address management.
* **Security Flow**: Issues a signed JWT upon login, validating it statelessly via the `Authorization` header on subsequent requests. Includes endpoints to check if an email exists and to fetch the current logged-in user.

### 4. Order Module (Checkout)
Manages the complete ordering process after cart checkout.
* **Features**: Basket-to-order conversion, server-side price validation (preventing client-side tampering), and delivery method selection.
* **Composite Entities**: The `Order` entity aggregates `OrderStatus`, `ShippingAddress` (Owned Entity Type), `DeliveryMethod`, and a collection of `OrderItems`.

### 5. Payment Module (Stripe Integration)
A decoupled module handling highly secure financial transactions via **Stripe**.
* **PaymentIntent**: Prepares payments by calculating totals server-side, tracking the lifecycle, and returning a secure `ClientSecret` to the frontend.
* **Webhooks**: Exposes a signature-verified endpoint (`/api/Payments/webhook`) that listens to Stripe events (e.g., `payment_intent.succeeded` or `payment_intent.payment_failed`) to automatically synchronize the Order Status without relying on the client.

### 6. Caching & Performance
* **Response Caching**: Improves performance and reduces database load.
* **Action Filters**: Intercepts requests for validation, logging, and caching before or after a controller action executes, keeping business logic clean.

---

## 💻 Technology Stack
* **Framework**: ASP.NET Core Web API
* **Database**: SQL Server (Entity Framework Core)
* **Caching**: Redis
* **Authentication**: ASP.NET Core Identity & JWT
* **Payment Gateway**: Stripe API & Webhooks
* **Mapping**: AutoMapper
* **Testing & Documentation**: Swagger UI, Postman, and `.http` files
"""

with open("E_Commerce_API_Presentation.md", "w", encoding="utf-8") as f:
    f.write(md_content)

print("Updated successfully.")
