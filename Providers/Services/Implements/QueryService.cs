using System.Linq.Expressions;
using Features.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Common.Enums;
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
public class QueryService: IQueryService
{
    /// <summary>
    /// 로거
    /// </summary>
    private readonly ILogger<IQueryService> _logger;
    
    /// <summary>
    /// DB Context
    /// </summary>
    private readonly AnalysisDbContext _dbContext;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="logger">로거</param>
    /// <param name="dbContext">dbContext</param>
    public QueryService(ILogger<IQueryService> logger, AnalysisDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    /// <summary>
    /// 요청정보로 쿼리를 파싱한다.
    /// </summary>
    /// <param name="requestQuery">요청정보</param>
    /// <returns>IQueryable</returns>
    public IQueryable<T> ReProductQuery<T>(RequestQuery requestQuery) where T : class 
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

            // 검색 메타 정보 
            List<RequestQuerySearchMeta> metas = requestQuery.SearchMetas;
            
            // 검색어 정보에 대해 처리
            foreach (var tuple in searchValue)
            {
                // 검색어가 일치하는 메타정보를 찾는다.
                RequestQuerySearchMeta? findMeta = metas.Find(i => i.Field.Equals(tuple.Item1, StringComparison.CurrentCultureIgnoreCase));
                
                // 찾지 못한경우 
                if(findMeta == null)
                    continue;
                
                ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
                MemberExpression property = Expression.Property(parameter, findMeta.Field);
                ConstantExpression constant = Expression.Constant(tuple.Item2);
                Expression condition;

                // 타입별 검색
                switch (findMeta.SearchType)
                {
                    // 동일한 값을 찾는경우 
                    case EnumQuerySearchType.Equals:
                        condition = Expression.Equal(property, constant);
                        break;
                    // 포함된 값을 찾는 경우 
                    case EnumQuerySearchType.Contains:
                        var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                        condition = Expression.Call(property, method, constant);
                        break;
                    default:
                        continue;
                }

                var lambda = Expression.Lambda<Func<T, bool>>(condition, parameter);
                conditions.Add(lambda);
            }
            
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
    /// ResponseList 로 변환한다.
    /// </summary>
    /// <param name="requestQuery">요청정보</param>
    /// <param name="mappingFunction"></param>
    /// <returns></returns>
    public async Task<ResponseList<V>> ToResponseListAsync<T,V>(RequestQuery requestQuery, Expression<Func<T, V>> mappingFunction)
        where T : class
        where V : class
    {
        ResponseList<V>? result;

        try
        {
            // 쿼리를 처리한다.
            QueryContainer<T>? container = await ToProductAsync<T>(requestQuery);
            
            // 쿼리 반환에 실패한경우
            if (container == null)
                throw new Exception("데이터베이스 처리중 예외가 발생했습니다.");

            // 조회
            List<V> list = await ToListAsync(container.Queryable, mappingFunction);
            
            return new ResponseList<V>(EnumResponseResult.Success, requestQuery, list, container.TotalCount);
        }
        catch (Exception e)
        {
            result = new ResponseList<V>("처리중 예외가 발생했습니다.");
            e.LogError(_logger);
        }

        return result;
    }

    /// <summary>
    /// 요청정보로 쿼리를 파싱한다.
    /// </summary>
    /// <param name="requestQuery">요청정보</param>
    /// <returns></returns>
    public async Task<QueryContainer<T>?> ToProductAsync<T>(RequestQuery requestQuery) where T : class
    {
        QueryContainer<T>? result;

        try
        {
            // 쿼리를 기본 재가공한다.
            IQueryable<T> query = ReProductQuery<T>(requestQuery);
            
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
    /// 최종 데이터를 매핑한다.
    /// </summary>
    /// <param name="queryable">IQueryable</param>
    /// <param name="mappingFunction">매핑 Delegate</param>
    /// <typeparam name="V">결과</typeparam>
    /// <returns></returns>
    public async Task<List<V>> ToListAsync<T,V>(IQueryable<T> queryable, Expression<Func<T, V>> mappingFunction)
        where T : class
        where V : class
    {
        List<V> result;

        try
        {
            return await queryable.Select(mappingFunction).ToListAsync();
        }
        catch (Exception e)
        {
            result = [];
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