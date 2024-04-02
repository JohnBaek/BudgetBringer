
using Features.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.DataModels;
using Models.Requests.Budgets;
using Models.Requests.Query;
using Models.Responses;
using Providers.Repositories.Interfaces;

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
    /// 생성자
    /// </summary>
    /// <param name="logger">로거</param>
    /// <param name="dbContext">디비컨텍스트</param>
    /// <param name="logActionRepository">로그액션 리파지토리</param>
    public BusinessUnitRepository(ILogger<BusinessUnitRepository> logger, AnalysisDbContext dbContext, ILogActionRepository logActionRepository)
    {
        _logger = logger;
        _dbContext = dbContext;
        _logActionRepository = logActionRepository;
    }


    /// <summary>
    /// 리스트를 가져온다.
    /// </summary>
    /// <param name="requestQuery">쿼리 정보</param>
    /// <returns></returns>
    public async Task<List<DbModelBusinessUnit>> GetListAsync(RequestQuery requestQuery)
    {
        try
        {
            var query = _dbContext.BusinessUnits.AsNoTracking()
                .Skip(requestQuery.Skip)
                .Take(requestQuery.PageCount);
        }
        catch (Exception e)
        {
            e.LogError(_logger);
        }

        return new List<DbModelBusinessUnit>();
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
}