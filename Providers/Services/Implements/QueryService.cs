using System.Linq.Expressions;
using System.Reflection;
using Features.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
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
    public IQueryable<T>? ReProductQuery<T>(RequestQuery requestQuery) where T : class 
    {
        IQueryable<T>? queryable;

        try
        {
            // 기본 셀렉팅 검색 쿼리를 Lazy 
            queryable = _dbContext.Set<T>().AsNoTracking();
          
            // 검색 요청정보를 가공한다.
            List<Expression<Func<T, bool>>> conditions = CreateSearchConditions<T>( requestQuery );
            
            // 검색 정보가 하나이상 존재한다면
            if(conditions.Count > 0)
                queryable = conditions.Aggregate(queryable, (current, condition) => current.Where(condition));
            
            // 소팅 요청정보를 가공한다.
            List<Expression<Func<IQueryable<T>, IOrderedQueryable<T>>>> sortOrders = CreateSortOrders<T>(requestQuery);
            
            // 소팅 정보가 하나이상 존재한다면
            if(sortOrders.Count > 0)
                // 모든 소팅 조건에 대해 처리
                queryable = sortOrders.Aggregate(queryable, (current, sortOrder) => sortOrder.Compile()(current));
        }
        catch (Exception e)
        {
            queryable = null;
            e.LogError(_logger);
        }

        return queryable;
    }
    
    
    /// <summary>
    /// 메타정보로 Where 검색 정보를 만든다.
    /// </summary>
    /// <param name="requestQuery"></param>
    private List<Expression<Func<T, bool>>> CreateSearchConditions<T>(RequestQuery requestQuery) 
        where T : class
    {
        List<Expression<Func<T, bool>>> conditions = new List<Expression<Func<T, bool>>>();

        try
        {
            // 사용자가 보낸 검색정보를 가공한다.
            IEnumerable<QuerySearch> searchRequests = ConvertToQuerySearchList(requestQuery);
            
            // 모든 검색정보에 대해 처리한다.
            foreach (QuerySearch search in searchRequests)
            {
                // 메타정보와 일치하는 검색정보를 찾는다.
                RequestQuerySearchMeta? findMeta = requestQuery.SearchMetas
                    .Find(i => i.Field.Equals(search.Field, StringComparison.CurrentCultureIgnoreCase));

                // 일치하는 정보가 없는 경우 
                if(findMeta == null)
                    continue;

                ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
                MemberExpression property = Expression.Property(parameter, findMeta.Field);
                ConstantExpression constant = Expression.Constant(search.Keyword);
                Expression condition;

                // 검색 타입별로 정리한다.
                switch (findMeta.SearchType)
                {
                    // 동일 검색 인경우 
                    case EnumQuerySearchType.Equals:
                        // 리플랙션 으로 타입을 찾아와 데이터베이스 컬럼과 맞춘다.
                        constant = GetParseConstant(search, property.Type);
                        condition = Expression.Equal(property, constant);
                        break;
                    
                    // Like 검색 인경우
                    case EnumQuerySearchType.Contains:
                        if (property.Type != typeof(string)) 
                        {
                            throw new InvalidOperationException($"'Contains' search is only supported for string properties. Property '{findMeta.Field}' is of type '{property.Type}'.");
                        }
                        MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;
                        condition = Expression.Call(property, method, constant);
                        break;
                    default:
                        continue;
                }
                var lambda = Expression.Lambda<Func<T, bool>>(condition, parameter);
                conditions.Add(lambda);
            }
        }
        catch (Exception e)
        {
            e.LogError(_logger);
            throw;
        }
        return conditions;
    }
    
    /// <summary>
    /// 메타정보로 OrderBy 정렬 정보를 만든다.
    /// </summary>
    /// <typeparam name="T">Entity의 타입</typeparam>
    /// <param name="requestQuery">정렬 정보가 포함된 요청 쿼리</param>
    private List<Expression<Func<IQueryable<T>, IOrderedQueryable<T>>>> CreateSortOrders<T>(RequestQuery requestQuery) 
    where T : class
    {
        List<Expression<Func<IQueryable<T>, IOrderedQueryable<T>>>> orders = new List<Expression<Func<IQueryable<T>, IOrderedQueryable<T>>>>();

        // 사용자가 보낸 Order 정보를 가공한다.
        IEnumerable<QuerySortOrder> sortOrders = ConvertToQuerySortList(requestQuery);

        try
        {
            foreach (QuerySortOrder sortOrder in sortOrders)
            {
                Type entityType = typeof(T);

                // IQueryable<T>에 대한 파라미터 표현식
                ParameterExpression queryParameter = Expression.Parameter(typeof(IQueryable<T>), "q");

                PropertyInfo? propertyInfo = entityType.GetProperties()
                    .FirstOrDefault(i => i.Name.Equals(sortOrder.Field, StringComparison.CurrentCultureIgnoreCase));

                if(propertyInfo == null)
                    throw new InvalidOperationException($"No property '{sortOrder.Field}' found on type '{entityType.Name}'.");

                // T의 인스턴스에 대한 파라미터 표현식
                ParameterExpression entityParameter = Expression.Parameter(entityType, "x");
                MemberExpression property = Expression.Property(entityParameter, propertyInfo);

                LambdaExpression propertyAccessLambda = Expression.Lambda(property, entityParameter);

                MethodInfo orderByMethod = typeof(Queryable).GetMethods().First(
                    method => method.Name == (sortOrder.Order == EnumQuerySortOrder.Asc ? "OrderBy" : "OrderByDescending") && 
                    method.GetParameters().Length == 2).MakeGenericMethod(entityType, propertyInfo.PropertyType);

                // 정렬 메서드 호출 표현식
                var orderByExpression = Expression.Call(
                    typeof(Queryable),
                    sortOrder.Order == EnumQuerySortOrder.Asc ? "OrderBy" : "OrderByDescending",
                    new Type[] { entityType, propertyInfo.PropertyType },
                    queryParameter, 
                    propertyAccessLambda);

                // 최종 정렬 Expression을 추가한다.
                orders.Add(Expression.Lambda<Func<IQueryable<T>, IOrderedQueryable<T>>>(orderByExpression, queryParameter));
            }
        }
        catch (Exception e)
        {
            e.LogError(_logger); // 이 부분은 로깅을 위한 가상의 메소드 호출입니다.
        }

        return orders;
    }


    /// <summary>
    /// 해당 타입에 맞게 ConstantExpression 를 변환처리한다
    /// </summary>
    /// <param name="querySearch"></param>
    /// <param name="property"></param>
    /// <returns></returns>
    private ConstantExpression GetParseConstant(QuerySearch querySearch, Type property)
    {
        object value = null;

        // Bool 처리
        if (property == typeof(bool) && bool.TryParse(querySearch.Keyword, out bool boolValue))
        {
            value = boolValue;
        }
        // Int 처리
        else if (property == typeof(int) && int.TryParse(querySearch.Keyword, out int intValue))
        {
            value = intValue;
        }
        // Float 처리
        else if (property == typeof(float) && float.TryParse(querySearch.Keyword, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float floatValue))
        {
            value = floatValue;
        }
        // Double 처리
        else if (property == typeof(double) && double.TryParse(querySearch.Keyword, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double doubleValue))
        {
            value = doubleValue;
        }
        // Enum 처리
        else if (property.IsEnum)
        {
            try
            {
                if (querySearch.Keyword != null)
                    value = Enum.Parse(property, querySearch.Keyword, true); // 대소문자 구분 없이 파싱
            }
            catch (Exception)
            {
                value = 0;
            }
        }
    
        // 파싱된 값이 있는 경우, 해당 타입의 상수 표현식으로 반환
        if (value != null)
        {
            return Expression.Constant(value, property);
        }
        
        // 파싱에 실패하거나 지원되지 않는 타입인 경우, 원본 문자열을 사용
        // 이 경우, 적절한 예외 처리 또는 로깅을 고려할 수 있음
        return Expression.Constant(querySearch.Keyword, typeof(string));
    }

    /// <summary>
    /// ResponseList 로 변환한다.
    /// </summary>
    /// <param name="requestQuery">요청정보</param>
    /// <param name="mappingFunction"></param>
    /// <returns></returns>
    public async Task<ResponseList<TV>> ToResponseListAsync<T,TV>(RequestQuery requestQuery, Expression<Func<T, TV>> mappingFunction)
        where T : class
        where TV : class
    {
        ResponseList<TV>? result;

        try
        {
            // 쿼리를 처리한다.
            QueryContainer<T>? container = await ToProductAsync<T>(requestQuery);
            
            // 쿼리 반환에 실패한경우
            if (container == null)
                throw new Exception("데이터베이스 처리중 예외가 발생했습니다.");

            // 조회
            List<TV> list = await ToListAsync(container.Queryable, mappingFunction);
            
            return new ResponseList<TV>(EnumResponseResult.Success, requestQuery, list, container.TotalCount);
        }
        catch (Exception e)
        {
            result = new ResponseList<TV>("처리중 예외가 발생했습니다.");
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
    /// <typeparam name="TV">결과</typeparam>
    /// <returns></returns>
    public async Task<List<TV>> ToListAsync<T,TV>(IQueryable<T> queryable, Expression<Func<T, TV>> mappingFunction)
        where T : class
        where TV : class
    {
        List<TV> result;

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
    private IEnumerable<QuerySortOrder> ConvertToQuerySortList(RequestQuery requestQuery)
    {
        List<QuerySortOrder> result = [];
        
        try
        {
            // 카운트가 일치하지않는 경우 
            if (requestQuery.SortOrders != null && requestQuery.SortFields != null 
                                                    && requestQuery.SortFields.Count != requestQuery.SortOrders.Count)
            {
                throw new ArgumentException("요청 필드와 값의 수는 같아야 합니다.");
            }

            // 모든 필드에 처리한다.
            for (int i = 0; i < requestQuery.SortFields?.Count; i++)
            {
                // 값을 분리하여 추가한다.
                QuerySortOrder add = new QuerySortOrder
                {
                    Field = requestQuery.SortFields[i] ,
                    Order = ConvertStringToEnum(requestQuery.SortOrders![i])
                };
                result.Add(add);
            }
        }
        catch (Exception e)
        {
            e.LogError(_logger);
        }

        return result;
    }
    
    
    /// <summary>
    /// 소팅 Order 를 변환한다.
    /// </summary>
    /// <param name="sortOrder">오더정보</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public EnumQuerySortOrder ConvertStringToEnum(string sortOrder)
    {
        if (Enum.TryParse<EnumQuerySortOrder>(sortOrder, true, out var result))
        {
            return result;
        }
        else
        {
            throw new ArgumentException("Invalid sort order value.");
        }
    }
    

    /// <summary>
    /// 메타정보로 1:1 매칭하여 필드:값 으로 분리한다.
    /// </summary>
    /// <param name="requestQuery">요청 정보</param>
    /// <returns></returns>
    private IEnumerable<QuerySearch> ConvertToQuerySearchList(RequestQuery requestQuery)
    {
        List<QuerySearch> result = [];
        
        try
        {
            // 카운트가 일치하지않는 경우 
            if (requestQuery.SearchKeywords != null && requestQuery.SearchFields != null 
                                                    && requestQuery.SearchFields.Count != requestQuery.SearchKeywords.Count)
            {
                throw new ArgumentException("요청 필드와 값의 수는 같아야 합니다.");
            }

            // 모든 필드에 처리한다.
            for (int i = 0; i < requestQuery.SearchFields?.Count; i++)
            {
                // 값을 분리하여 추가한다.
                QuerySearch add = new QuerySearch
                {
                    Field = requestQuery.SearchFields[i] ,
                    Keyword = requestQuery.SearchKeywords![i]
                };
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