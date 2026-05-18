# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build
dotnet build

# Run dev server (serves both API + Blazor WASM)
dotnet run --project Server/Ecommerce.Server.csproj
# https://localhost:7295  |  Swagger: https://localhost:7295/swagger

# Run all tests
dotnet test

# Run server tests only
dotnet test tests/Ecommerce.Server.Tests

# Run a single test by name
dotnet test --filter "FullyQualifiedName~MethodName"

# Add a migration
dotnet ef migrations add <Name> --project Server

# Apply migrations
dotnet ef database update --project Server
```

## Architecture

This is a **hosted Blazor WASM** solution — the ASP.NET Core server (`Server`) hosts and serves the Blazor WebAssembly app (`Client`) as static files, while also exposing the REST API the client calls.

**Projects:**
- `Server` — ASP.NET Core 8 API + Blazor host. All business logic, EF Core, Identity, JWT.
- `Client` — Blazor WASM SPA. Runs entirely in the browser, calls the server API via `HttpClient`.
- `Shared` — DTOs and request/response models referenced by both `Server` and `Client`.
- `tests/Ecommerce.Server.Tests` — xUnit integration + unit tests for the server.

## Key Patterns

**Service layer:** Controllers are thin HTTP handlers. All business logic lives in `Server/Services/`. Each service has an interface (`IProductService`, etc.) registered in `Program.cs` as scoped.

**Soft deletes:** Products and categories are never physically deleted — `IsActive = false` flags them as deleted. Queries must filter on `IsActive`.

**Auth flow:**
1. Client posts credentials to `POST /api/auth/login` → receives a JWT.
2. JWT is stored in browser localStorage (`"authToken"` key) via `LocalStorageService`.
3. `AuthTokenHandler` (a `DelegatingHandler`) automatically injects `Authorization: Bearer <token>` on every outbound `HttpClient` request.
4. `CustomAuthStateProvider` parses the JWT client-side (no server round-trip) to populate the `AuthenticationState`. It checks expiry and removes stale tokens.
5. Server validates JWTs via standard `AddJwtBearer` middleware; roles are embedded as claims.

**Role-based access:** Two roles — `Admin` and `Customer`. Admin endpoints use `[Authorize(Roles = "Admin")]`. Default admin seed: `admin@ecommerce.com` / `Admin@123!`.

**User identity in controllers:** Extract the current user's ID with `User.FindFirstValue(ClaimTypes.NameIdentifier)`.

**Image storage:** Product images are uploaded to Azure Blob Storage (`product-images` container). `BlobStorageService` uploads and generates a 10-year SAS read URI. The blob name is stored alongside the URL in `Product.ImageBlobName` so old blobs can be deleted on update.

**Seed data:** `DataSeeder.SeedAsync` runs on every startup. It calls `MigrateAsync()` for relational providers or `EnsureCreatedAsync()` for InMemory (tests). Categories/products are seeded via EF `HasData()` in `ApplicationDbContext.OnModelCreating`.

## Testing

Integration tests use `TestWebApplicationFactory` which:
- Swaps SQL Server for `UseInMemoryDatabase` (unique DB name per test instance).
- Replaces `IBlobStorageService` with a no-op `StubBlobStorageService`.

Unit tests (under `tests/Ecommerce.Server.Tests/Services/`) use Moq to mock `ApplicationDbContext` dependencies directly.

## Configuration

Required keys in `appsettings.json` (or environment overrides):

| Key | Purpose |
|-----|---------|
| `ConnectionStrings:DefaultConnection` | SQL Server connection string |
| `Jwt:Key` | HMAC-SHA256 signing key (≥32 chars) |
| `Jwt:Issuer` / `Jwt:Audience` | JWT validation values |
| `Jwt:ExpiresHours` | Token lifetime (default `24`) |
| `BlobStorage:ConnectionString` | Azure Storage account |
| `BlobStorage:ContainerName` | Blob container (default `product-images`) |
