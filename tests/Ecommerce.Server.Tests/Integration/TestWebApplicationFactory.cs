using Ecommerce.Server.Data;
using Ecommerce.Server.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ecommerce.Server.Tests.Integration;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _dbName = "TestDb_" + Guid.NewGuid();

    protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Replace SQL Server DbContext with InMemory
            services.RemoveAll<DbContextOptions<ApplicationDbContext>>();
            services.AddDbContext<ApplicationDbContext>(opts =>
                opts.UseInMemoryDatabase(_dbName));

            // Replace BlobStorageService with a no-op stub
            services.RemoveAll<IBlobStorageService>();
            services.AddScoped<IBlobStorageService, StubBlobStorageService>();
        });
    }
}

public class StubBlobStorageService : IBlobStorageService
{
    public Task<(string Url, string BlobName)> UploadAsync(Stream stream, string fileName, string contentType) =>
        Task.FromResult(($"https://stub.blob.core.windows.net/{fileName}", fileName));

    public Task DeleteAsync(string blobName) => Task.CompletedTask;
}
