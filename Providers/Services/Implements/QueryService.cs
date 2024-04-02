using System.Linq.Expressions;
using Features.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Common.Query;
using Models.DataModels;
using Models.Requests.Query;
using Models.Responses;
using Providers.Services.Interfaces;

namespace Providers.Services.Implements;

/// <summary>
/// 쿼리서비스 구현체
/// </summary>
/// <typeparam name="T"></typeparam>
public class QueryService<T> : IQueryService<T> where T : class
{
    /// <summary>
    /// 로거
    /// </summary>
    private readonly ILogger<IQueryService<T>> _logger;
    
    /// <summary>
    /// DB Context
    /// </summary>
    private readonly AnalysisDbContext _dbContext;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="logger">로거</param>
    /// <param name="dbContext">dbContext</param>
    public QueryService(ILogger<IQueryService<T>> logger, AnalysisDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    /// <summary>
    /// 요청정보로 쿼리를 파싱한다.
    /// </summary>
    /// <param name="requestQuery">요청정보</param>
    /// <returns>IQueryable</returns>
    public IQueryable<T> ReProductQuery(RequestQuery requestQuery)
    {
        IQueryable<T>? result;

        try
        {
            // 쿼리를 생성한다.
            IQueryable<T> query = _dbContext.Set<T>().AsNoTracking();
            
            // 조건 설정
            List<Expression<Func<T, bool>>> conditions = [];
            
            // 요청정보를 가공한다.
            IEnumerable<Tuple<string, string>> searchValue = ConvertToQuerySearchList(requestQuery);
            
            // 모든 조건에 대해 추가처리
            result = conditions.Aggregate(query, (current, condition) => current.Where(condition));
        }
        catch (Exception e)
        {
            result = null;
            e.LogError(_logger);
        }

        return result;
    }

    /// <summary>
    /// 요청정보로 쿼리를 파싱한다.
    /// </summary>
    /// <param name="requestQuery">요청정보</param>
    /// <returns></returns>
    public async Task<QueryContainer<T>?> ToProductAsync(RequestQuery requestQuery)
    {
        QueryContainer<T>? result;

        try
        {
            // 쿼리를 기본 재가공한다.
            IQueryable<T> query = this.ReProductQuery(requestQuery);
            
            // 전체 카운트를 구한다.
            int totalCount = await query.CountAsync();

            // 현재 조건으로 쿼리를 적용한다.
            query = query.Skip(requestQuery.Skip).Take(requestQuery.PageCount);

            return new QueryContainer<T>(totalCount, query, requestQuery);
        }
        catch (Exception e)
        {
            result = null;
            e.LogError(_logger);
        }

        return result;
    }

    /// <summary>
    /// 메타정보로 1:1 매칭하여 필드:값 으로 분리한다.
    /// </summary>
    /// <param name="requestQuery">요청 정보</param>
    /// <returns></returns>
    private IEnumerable<Tuple<string, string>> ConvertToQuerySearchList(RequestQuery requestQuery)
    {
        List<Tuple<string,string>> result = [];
        
        try
        {
            // 카운트가 일치하지않는 경우 
            if (requestQuery.SearchFields.Count != requestQuery.SearchKeywords.Count)
            {
                throw new ArgumentException("요청 필드와 값의 수는 같아야 합니다.");
            }

            // 모든 필드에 처리한다.
            for (int i = 0; i < requestQuery.SearchFields.Count; i++)
            {
                // 값을 분리하여 추가한다.
                Tuple<string, string> add = new Tuple<string, string>(requestQuery.SearchFields[i],requestQuery.SearchKeywords[i]);
                result.Add(add);
            }
        }
        catch (Exception e)
        {
            e.LogError(_logger);
        }

        return result;
    }
}