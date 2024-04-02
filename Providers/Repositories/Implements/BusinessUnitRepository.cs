
using System.Linq.Expressions;
using Features.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Common.Enums;
using Models.Common.Query;
using Models.DataModels;
using Models.Requests.Budgets;
using Models.Requests.Query;
using Models.Responses;
using Models.Responses.Budgets;
using Providers.Repositories.Interfaces;
using Providers.Services.Interfaces;

namespace Providers.Repositories.Implements;

/// <summary>
/// 비지니스 유닛 Repository 구현체
/// </summary>
public class BusinessUnitRepository : IBusinessUnitRepository
{
    /// <summary>
    /// DB Context
    /// </summary>
    private readonly AnalysisDbContext _dbContext;

    /// <summary>
    /// 로거
    /// </summary>
    private readonly ILogger<BusinessUnitRepository> _logger;

    /// <summary>
    /// 로그액션 리파지토리
    /// </summary>
    private readonly ILogActionRepository _logActionRepository;

    /// <summary>
    /// 쿼리 서비스
    /// </summary>
    private readonly IQueryService<DbModelBusinessUnit> _queryService;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="logger">로거</param>
    /// <param name="dbContext">디비컨텍스트</param>
    /// <param name="logActionRepository">로그액션 리파지토리</param>
    /// <param name="queryService"></param>
    public BusinessUnitRepository(
          ILogger<BusinessUnitRepository> logger
        , AnalysisDbContext dbContext
        , ILogActionRepository logActionRepository
        , IQueryService<DbModelBusinessUnit> queryService)
    {
        _logger = logger;
        _dbContext = dbContext;
        _logActionRepository = logActionRepository;
        _queryService = queryService;
    }


    /// <summary>
    /// 리스트를 가져온다.
    /// </summary>
    /// <param name="requestQuery">쿼리 정보</param>
    /// <returns></returns>
    public async Task<ResponseList<ResponseBusinessUnit>> GetListAsync(RequestQuery requestQuery)
    {
        ResponseList<ResponseBusinessUnit> result = new ResponseList<ResponseBusinessUnit>();
        try
        {
            // 쿼리 설정
            QueryContainer<DbModelBusinessUnit>? container = await _queryService.ToProductAsync(requestQuery);

            // 쿼리 반환에 실패한경우
            if (container == null)
                return new ResponseList<ResponseBusinessUnit>("데이터베이스 처리중 예외가 발생했습니다.");

            // 실제 셀렉팅
            List<ResponseBusinessUnit> list = await container.Queryable
                .Select(item => new ResponseBusinessUnit
                {
                    Id = item.Id,
                    Name = item.Name
                })
                .ToListAsync();

            // 결과를 바인딩한다.
            result = new ResponseList<ResponseBusinessUnit>(EnumResponseResult.Success, requestQuery, list, container.TotalCount);
        }
        catch (Exception e)
        {
            e.LogError(_logger);
        }

        return result;
    }

    /// <summary>
    /// 데이터를 업데이트한다.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Response> UpdateAsync(RequestBusinessUnit request)
    {
        Response result;
        
        try
        {
            result = new Response();
        }
        catch (Exception e)
        {
            result = new Response();
            e.LogError(_logger);
        }
    
        return result;
    }
    
    /// <summary>
    /// 데이터를 추가한다.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Response> AddAsync(RequestBusinessUnit request)
    {
        Response result;
        
        try
        {
            result = new Response();
        }
        catch (Exception e)
        {
            result = new Response();
            e.LogError(_logger);
        }
    
        return result;
    }
    
    /// <summary>
    /// 데이터를 삭제한다.
    /// </summary>
    /// <param name="id">대상 아이디값</param>
    /// <returns></returns>
    public async Task<Response> DeleteAsync(string id)
    {
        Response result;
        
        try
        {
            result = new Response();
        }
        catch (Exception e)
        {
            result = new Response();
            e.LogError(_logger);
        }
    
        return result;
    }
    
    
    public List<QuerySearch> ConvertToQuerySearchList(RequestQuery requestQuery)
    {
        var querySearchList = new List<QuerySearch>();

        if (requestQuery.SearchFields.Count != requestQuery.SearchKeywords.Count)
        {
            throw new ArgumentException("Fields and keywords lists should have the same length.");
        }

        for (int i = 0; i < requestQuery.SearchFields.Count; i++)
        {
            querySearchList.Add(new QuerySearch
            {
                Field = requestQuery.SearchFields[i],
                Value = requestQuery.SearchKeywords[i]
            });
        }

        return querySearchList;
    }
}


public class QuerySearch
{
    public string Field { get; set; }
    public string Value { get; set; }
}
