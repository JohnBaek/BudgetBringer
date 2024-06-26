using Models.DataModels;

namespace Providers.Services.Interfaces;

/// <summary>
/// SystemConfig Service
/// </summary>
public interface ISystemConfigService
{
    /// <summary>
    /// Get Config by config Name and details config name 
    /// </summary>
    /// <param name="configName"></param>
    /// <param name="detailConfig"></param>
    Task<T?> GetValueAsync<T>(string configName, string detailConfig);

    /// <summary>
    /// Get Config by config Name and details config name
    /// </summary>
    /// <param name="configName"></param>
    /// <param name="detailConfig"></param>
    /// <returns></returns>
    Task<DbModelSystemConfigDetail?> GetValueAsync(string configName, string detailConfig);
}