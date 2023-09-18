using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Products.Persistence.HealthChecks;

public class ProductServiceHealthCheck: IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        return await Task.FromResult(HealthCheckResult.Healthy("Product service is working"));
    }
}