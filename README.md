# 🛒 E-Commerce RESTful API

<div align="center">
  <h3>An Enterprise-Level ASP.NET Core Web API Project</h3>
  <p><em>Built with Clean Architecture (Onion Architecture) principles.</em></p>
  <p>Developed by <b>Moaaz Atef</b></p>
</div>

---

## 📌 Project Agenda & Overview

This repository contains a robust RESTful API built for an E-Commerce platform. The project is designed with scalability, maintainability, and clean architecture in mind, separating concerns across distinct layers.

### 🛠️ Key Architectural Highlights
1. **API Fundamentals**: Follows strict RESTful standards (Resource-based endpoints, HTTP verbs, and Stateless JSON responses).
2. **Onion Architecture**: 
   * **Domain Layer** (Center): Entities & Contracts (Interfaces). Depends on nothing.
   * **Application Layer**: Orchestration, Service Interfaces, and DTOs.
   * **Infrastructure Layer**: EF Core DbContext, Repositories, Migrations, and Data Seeding.
   * **Presentation Layer (API)**: Web-facing edge, Controllers, and Composition Root (DI).
3. **Product Module**: The core vertical slice managing the catalog of Products, Brands, and Types.

---

## 🧅 Architecture Breakdown (The 4 Projects)

The solution is split into 4 distinct projects ensuring dependencies flow **inward only**:

```text
📂 E_Commerce.sln
├── 📁 E_Commerce.Domain         // 🎯 Entities + Contracts (no dependencies)
├── 📁 E_Commerce.Application    // ⚙️ Services & DTOs (depends on Domain)
├── 📁 E_Commerce.Infrastructure // 🗄️ EF Core, Repos, Seeding (depends on Domain & Application)
└── 📁 E_Commerce.API            // 🌐 Controllers, Program.cs (Composition Root)
```

---

## 📦 Product Module

The Product Module is the first vertical slice of the architecture, handling the core product catalog.

### 🗃️ Entities & Relationships
* **`Product`**: The core entity (Id, Name, Description, PictureUrl, Price).
* **`ProductBrand`**: Lookup entity for brands (e.g., Nike, Sony).
* **`ProductType`**: Lookup entity for categories (e.g., Boards, Hats).
* **Relationships**: Each `Product` belongs to exactly one `Brand` and one `Type` (One-to-Many).

### 📡 Endpoints (Read-Only)

| Verb   | Route | Description | Response |
| :--- | :--- | :--- | :--- |
| **GET** | `/api/products` | Retrieves all products | `200 OK` · `List<Product>` |
| **GET** | `/api/products/{id}` | Retrieves a specific product by ID | `200 OK` · `Product` or `404 Not Found` |
| **GET** | `/api/products/brands` | Retrieves all product brands | `200 OK` · `List<ProductBrand>` |
| **GET** | `/api/products/types`| Retrieves all product types | `200 OK` · `List<ProductType>` |

---

## 💻 Technologies & Concepts Used
* **ASP.NET Core Web API**
* **Entity Framework Core** (Fluent API Configurations)
* **Dependency Injection (DI)**
* **Repository & Unit of Work Patterns**
* **Data Seeding**
