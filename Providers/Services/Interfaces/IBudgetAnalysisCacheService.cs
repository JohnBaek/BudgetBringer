using Models.Common.Enums;
using Models.Requests.Login;
using Models.Responses;
using Models.Responses.Users;

namespace Providers.Services.Interfaces;

/// <summary>
/// 캐시 서비스
/// </summary>
public interface IBudgetAnalysisCacheService
{
    /// <summary>
    /// 캐시가 있는지 확인한다.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    Task<ResponseData<bool>> HasCachedAsync(EnumBudgetAnalysisCacheType type);
    
    /// <summary>
    /// 캐시를 추가한다.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    Task<Response> AddCacheAsync(EnumBudgetAnalysisCacheType type, object target);

    /// <summary>
    /// 시리얼라이즈된 통계 데이터를 리턴한다.
    /// </summary>
    /// <param name="type"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<ResponseData<T>> GetDeserializedData<T>(EnumBudgetAnalysisCacheType type) where T : class;
}