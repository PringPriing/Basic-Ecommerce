# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files first for layer-cache efficiency
COPY Ecommerce.sln ./
COPY Server/Ecommerce.Server.csproj       Server/
COPY Client/Ecommerce.Client.csproj       Client/
COPY Shared/Ecommerce.Shared.csproj       Shared/

RUN dotnet restore Server/Ecommerce.Server.csproj

# Copy full source and publish (Client WASM is built automatically as a Server dependency)
COPY . .
RUN dotnet publish Server/Ecommerce.Server.csproj -c Release -o /app/publish --no-restore

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Ecommerce.Server.dll"]
