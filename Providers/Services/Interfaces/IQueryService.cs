using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Models.Common.Query;
using Models.Requests.Query;
using Models.Responses;

namespace Providers.Services.Interfaces;

/// <summary>
/// 쿼리 서비스 인터페이스 
/// </summary>
public interface IQueryService
{
    /// <summary>
    /// 요청정보로 쿼리를 파싱한다.
    /// </summary>
    /// <param name="requestQuery">요청정보</param>
    /// <returns></returns>
    IQueryable<T>? ReProductQuery<T>(RequestQuery requestQuery) where T : class;


    /// <summary>
    /// ResponseList 로 변환한다.
    /// </summary>
    /// <param name="requestQuery">요청정보</param>
    /// <param name="mappingFunction"></param>
    /// <returns></returns>
    Task<ResponseList<V>> ToResponseListAsync<T,V>(RequestQuery requestQuery, Expression<Func<T, V>> mappingFunction)
        where T : class
        where V : class;
    
    /// <summary>
    /// 요청정보로 쿼리를 파싱한다.
    /// </summary>
    /// <param name="requestQuery">요청정보</param>
    /// <returns></returns>
    Task<QueryContainer<T>?> ToProductAsync<T>(RequestQuery requestQuery) where T : class;

    /// <summary>
    /// 최종 데이터를 매핑한다.
    /// </summary>
    /// <param name="queryable">IQueryable</param>
    /// <param name="mappingFunction">매핑 Delegate</param>
    /// <typeparam name="V">결과</typeparam>
    /// <returns></returns>
    Task<List<V>> ToListAsync<T,V>(IQueryable<T> queryable, Expression<Func<T, V>> mappingFunction)
        where T : class
        where V : class;
}