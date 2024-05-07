using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.DataModels;
using Providers.Services.Interfaces;

namespace Providers.Services.Implements;

/// <summary>
/// Implements of SystemConfig Service
/// </summary>
public class SystemConfigService : ISystemConfigService
{
    /// <summary>
    /// DB Context
    /// </summary>
    private readonly AnalysisDbContext _dbContext;
    
    /// <summary>
    /// Logger
    /// </summary>
    private readonly ILogger<SystemConfigService> _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="logger"></param>
    public SystemConfigService(AnalysisDbContext dbContext, ILogger<SystemConfigService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Get Config by config Name and details config name 
    /// </summary>
    /// <param name="configName"></param>
    /// <param name="detailConfig"></param>
    /// <returns></returns>
    public async Task<T?> GetValueAsync<T>(string configName, string detailConfig)
    {
        T? result = default(T);
        
        try
        {
            // Find config
            var config = await _dbContext.SystemConfigs
                .AsNoTracking()
                .Include(i => i.Details)
                .Where(i => i.Name == configName)
                .SelectMany(i => i.Details!)
                .FirstOrDefaultAsync(d => d.Name == detailConfig);

            if (config != null)
            {
                // Attempt to convert the value to the specified type T
                return (T) Convert.ChangeType(config.Value, typeof(T));
            }
        }
        catch (InvalidCastException e)
        {
            _logger.LogError(e, "Failed to convert configuration value to type {Type}", typeof(T));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error retrieving or converting the configuration value");
        }

        return result;
    }
}