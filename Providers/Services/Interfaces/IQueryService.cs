using Microsoft.EntityFrameworkCore;
using Models.Common.Query;
using Models.Requests.Query;
using Models.Responses;

namespace Providers.Services.Interfaces;

/// <summary>
/// 쿼리 서비스 인터페이스 
/// </summary>
public interface IQueryService<T> where T : class
{
    /// <summary>
    /// 요청정보로 쿼리를 파싱한다.
    /// </summary>
    /// <param name="requestQuery">요청정보</param>
    /// <returns></returns>
    IQueryable<T>? ReProductQuery(RequestQuery requestQuery);
    
    
    /// <summary>
    /// 요청정보로 쿼리를 파싱한다.
    /// </summary>
    /// <param name="requestQuery">요청정보</param>
    /// <returns></returns>
    Task<QueryContainer<T>?> ToProductAsync(RequestQuery requestQuery);
}