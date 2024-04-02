using Features.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Common.Enums;
using Models.DataModels;
using Models.Requests.Query;
using Models.Responses;

namespace Providers.Repositories.Implements;

/// <summary>
/// 액션 로그 모델 구현체
/// </summary>
public class LogActionRepository : ILogActionRepository
{
    /// <summary>
    /// DB Context
    /// </summary>
    private readonly AnalysisDbContext _dbContext;

    /// <summary>
    /// 로거
    /// </summary>
    private readonly ILogger<LogActionRepository> _logger;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="logger">로거</param>
    /// <param name="dbContext">디비컨텍스트</param>
    public LogActionRepository(ILogger<LogActionRepository> logger, AnalysisDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    
    /// <summary>
    /// 리스트를 가져온다.
    /// </summary>
    /// <param name="requestQuery">쿼리 정보</param>
    /// <returns></returns>
    public async Task<List<DbModelLogAction>> GetListAsync(RequestQuery requestQuery)
    {
        List<DbModelLogAction> result = [];
        
        try
        {
            result = await _dbContext.LogActions.AsNoTracking()
                .Skip(requestQuery.Skip)
                .Take(requestQuery.PageCount)
                .ToListAsync();
        }
        catch (Exception e)
        {
            e.LogError(_logger);
        }
    
        return result;
    }

    /// <summary>
    /// 로그를 추가한다.
    /// </summary>
    /// <param name="actionType">데이터베이스 액션 타입</param>
    /// <param name="contents">로그 컨텐츠</param>
    /// <returns></returns>
    public async Task<Response> AddAsync(EnumDatabaseLogActionType actionType, string contents)
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