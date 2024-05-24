// Middleware/ConfigMiddleware.cs
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using ProjectMangaSmurf.Services;
using ProjectMangaSmurf.Models;
using ProjectMangaSmurf.Repository;

public class ConfigMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfigService _configService;
    private readonly ILogger<ConfigMiddleware> _logger;

    public ConfigMiddleware(RequestDelegate next, IConfigService configService, ILogger<ConfigMiddleware> logger)
    {
        _next = next;
        _configService = configService;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            _logger.LogInformation("Fetching configuration values...");

            GlobalConfig.Config20 = await _configService.GetConfigValueAsync(20);
            GlobalConfig.Config1 = await _configService.GetConfigValueAsync(1);
            GlobalConfig.Config2 = await _configService.GetConfigValueAsync(2);

            _logger.LogInformation("Configuration values fetched successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching configuration values");
        }

        await _next(context);
    }
}
