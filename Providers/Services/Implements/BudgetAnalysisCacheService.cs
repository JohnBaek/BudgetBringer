using Features.Extensions;
using Microsoft.Extensions.Logging;
using Models.Common.Enums;
using Models.Responses;
using Providers.Repositories.Interfaces;
using Providers.Services.Interfaces;

namespace Providers.Services.Implements;

/// <summary>
/// 예산 분석 서비스
/// </summary>
public class BudgetAnalysisCacheService : IBudgetAnalysisCacheService
{
    /// <summary>
    /// 리파지토리
    /// </summary>
    private readonly IBudgetAnalysisCacheRepository _repository;
    
    /// <summary>
    /// Logger
    /// </summary>
    private readonly ILogger<BudgetAnalysisCacheService> _logger;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="logger"></param>
    public BudgetAnalysisCacheService(IBudgetAnalysisCacheRepository repository, ILogger<BudgetAnalysisCacheService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// 캐시가 있는지 확인한다.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public async Task<ResponseData<bool>> HasCachedAsync(EnumBudgetAnalysisCacheType type)
    {
        ResponseData<bool> response;
        
        try
        {
            response = await _repository.HasCachedAsync(type);
        }
        catch (Exception e)
        {
            response = new ResponseData<bool>(EnumResponseResult.Error,"","처리중 예외가 발생했습니다.",false);
            e.LogError(_logger);
        }

        return response;
    }

    /// <summary>
    /// 캐시를 추가한다.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public async Task<Response> AddCacheAsync(EnumBudgetAnalysisCacheType type, object target)
    {
        Response response;
        
        try
        {
            response = await _repository.AddCacheAsync(type , target);
        }
        catch (Exception e)
        {
            response = new Response(EnumResponseResult.Error,"","처리중 예외가 발생했습니다.");
            e.LogError(_logger);
        }

        return response;
    }

    /// <summary>
    /// 시리얼라이즈된 통계 데이터를 리턴한다.
    /// </summary>
    /// <param name="type"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<ResponseData<T>> GetDeserializedData<T>(EnumBudgetAnalysisCacheType type) where T : class
    {
        ResponseData<T> response;
        
        try
        {
            response = await _repository.GetDeserializedData<T>(type);
        }
        catch (Exception e)
        {
            response = new ResponseData<T>(EnumResponseResult.Error,"","처리중 예외가 발생했습니다.",null);
            e.LogError(_logger);
        }

        return response;
    }
}