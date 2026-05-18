using Azure.Storage.Blobs;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Ecommerce.Server.HealthChecks;

public class BlobStorageHealthCheck : IHealthCheck
{
    private readonly IConfiguration _config;

    public BlobStorageHealthCheck(IConfiguration config) => _config = config;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken ct = default)
    {
        try
        {
            var connStr = _config["BlobStorage:ConnectionString"];
            var containerName = _config["BlobStorage:ContainerName"];
            var client = new BlobContainerClient(connStr, containerName);
            await client.ExistsAsync(ct);
            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(ex.Message);
        }
    }
}
