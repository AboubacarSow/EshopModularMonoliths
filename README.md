# 🛒 EShop Modular Monolith

## 📌 Overview
EShop Modular Monolith is an advanced **.NET 8 REST API** project designed to demonstrate the implementation of a **modular monolithic architecture**.  
The solution follows an **evolved Vertical Slice Architecture** approach, focusing on feature-based separation and modular design principles.  

The project integrates multiple architectural patterns and modern practices to ensure scalability, maintainability, and clean code organization.

---

## ⚙️ Tech Stack
- **.NET 8** (Web API)
- **PostgreSQL** (Database for all modules and Keycloak)- each module has its own schema
- **Docker Desktop** (for API and all backing services)
- **Entity Framework Core** – ORM with Interceptors and Repository Pattern (Basket module)
- **Keycloak** (Authentication & Authorization)
- **Redis** (Caching & Distributed state management)
- **RabbitMQ + MassTransit**  (Messaging & Event-driven communication)
- **Serilog + Seq Sink** (Centralized logging & monitoring)

---

## 🏛️ Architecture & Patterns
- **Evolved Vertical Slice Architecture** (instead of Clean Architecture)
- **CQRS & Mediator Pattern** (with MediatR)
- **Pipeline Behaviors** (MediatR custom pipeline)
- **Repository Pattern** (used in Basket module)
- **Outbox Pattern** (reliable event publishing)
- **Proxy Pattern**
- **Decorator Pattern**
- **REPR Pattern** (Resource-Entity-Property-Representation)
- **EF Core Interceptors** (custom DB interception)

---

## 📦 Modules
Currently implemented business modules:
- **Basket Module**
- **Catalog Module**
- **Ordering Module**
- **Identity Module**

⚠️ **Note:** The project does not include a **Payment Module**.

---

## 🔑 Security
- Authentication & Authorization powered by **Keycloak**
- Centralized PostgreSQL database for Keycloak and modules
- JWT Token-based authentication flow 

---

## 📡 Backing Services
The project relies on several backing services running inside **Docker Desktop**:

- **PostgreSQL** – Relational database  
- **Redis** – Distributed cache  
- **RabbitMQ** – Message broker  
- **Serilog + Seq** – Logging & monitoring pipeline  
- **Keycloak** – Identity & Access Management  

---

## 🚀 Features
- Modular Monolithic design with vertical slices
- REST API (no versioning implemented yet)
- Asynchronous communication with RabbitMQ + Outbox pattern
- Distributed cache with Redis
- Advanced MediatR usage (CQRS, pipeline behaviors)
- Logging with Serilog & Seq
- Runs fully inside Docker (API + backing services)

---

## ❌ Limitations
- No **Payment Module**
- No **API versioning**
- No **automated testing** implemented yet

---

## 🛠️ Getting Started
### 🐳 Running the Project
1. Clone the repository  
   ```bash
      git clone https://github.com/AboubacarSow/EShopModularMonolith.git
    ```
2. Ensure **Docker Desktop** is installed and running  
3. Start services with:  
   ```bash
      docker-compose up -d
    ```
    
## 📄 License
This project is licensed under the MIT License. See the [LICENSE](LICENSE.txt) file for details.
