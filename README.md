
**Overview 🌐**
GadgetHub is split into two main projects:
- GadgetHub.Web.API — Backend REST API (data, business logic, auth) 🔐
- GadgetHub.Web.MVC — Frontend Razor-based web app (UI, sessions, server-rendered pages) 🖥️
**Tech stack 🧰**
- .NET 10 (C# 14.0) ⚙️
- ASP.NET Core Web API (GadgetHub.Web.API) for REST endpoints
- ASP.NET Core MVC / Razor Pages style frontend (GadgetHub.Web.MVC) for server-rendered views
- Entity Framework Core (SQL Server) for persistence (MyDBcontext) 🗄️
- AutoMapper for DTO ↔ domain mapping 🔁
- JWT Authentication (Bearer tokens) for API security 🔐
- Dependency Injection for services/repositories 🧩
- Swashbuckle / Swagger for API docs (/swagger) 📘
- HttpClient typed clients in GadgetHub.Web.MVC for calling the API (e.g. IProductApiClient) 🔗
- WebOptimizer for bundling/minifying CSS & JS (bundles: /css/bundle.css, /js/bundle.js) 🎛️
- Session support for the web app (server session) 🧾
**Project structure 📂**
- GadgetHub.Web.API/
- Program.cs — app bootstrap, JWT auth, DI registration, Swagger setup
- MyDBcontext — EF Core DbContext
- Controllers/ — API controllers
- Services/, Repositories/ — business & data access implementations
- GadgetHub.Web.MVC/
- Program.cs — web app bootstrap, HttpClient typed registrations, WebOptimizer, session
- Views/ — Razor views (server-rendered UI)
- HttpClients/ — typed API clients like ProductsApiClient, CategoriesApiClient
- Services/ — UI-side services such as TokenService
**Architecture (high-level) 🏗️**
- Clients (browser) → GadgetHub.Web.MVC (Razor pages, sessions)
- The frontend calls backend API via typed HttpClients. The frontend may store JWT in session and attach it when calling protected API endpoints.
- GadgetHub.Web.MVC → GadgetHub.Web.API (HTTP)
- API exposes endpoints for products, categories, authentication, etc.
- GadgetHub.Web.API → Database (EF Core / SQL Server)
- Repositories and DbContext handle persistence. Application layer contains services and mapping profiles.
Flow example:
- User logs in via GadgetHub.Web.MVC → LoginApiClient -> GadgetHub.Web.API issues JWT. 🔑
- GadgetHub.Web.MVC stores token in session via ITokenService. 🧾
- Frontend calls protected API endpoints using typed HttpClient and token in Authorization header. 🔗
**Configuration notes ⚙️**
- Database connection string key: "constr" in appsettings.json for GadgetHub.Web.API.
- JWT values expected in configuration:
- Jwt:Issuer, Jwt:Audience, Jwt:Key
- API base URL in GadgetHub.Web.MVC Program.cs config: e.g. https://localhost:44379/ (adjust for your environment).

