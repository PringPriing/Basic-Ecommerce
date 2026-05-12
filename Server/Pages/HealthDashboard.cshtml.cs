using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Ecommerce.Server.Pages;

public class HealthDashboardModel : PageModel
{
    private readonly HealthCheckService _healthCheckService;

    public HealthReport? Report { get; private set; }
    public DateTime CheckedAt { get; private set; }

    public HealthDashboardModel(HealthCheckService healthCheckService)
    {
        _healthCheckService = healthCheckService;
    }

    public async Task OnGetAsync()
    {
        Report = await _healthCheckService.CheckHealthAsync();
        CheckedAt = DateTime.UtcNow;
    }
}
