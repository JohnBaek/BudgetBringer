using Features.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Common.Enums;
using Models.DataModels;
using Models.Requests.Query;
using Models.Responses;
using Providers.Repositories.Interfaces;
using Providers.Services.Interfaces;

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
    /// 사용자 리파지토리
    /// </summary>
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// 쿼리 서비스
    /// </summary>
    private readonly IQueryService _queryService;

    /// <summary>
    /// 액션 로그 기록 서비스
    /// </summary>
    private readonly ILogActionWriteService _logActionWriteService;

    /// <summary>
    /// 로그 카테고리명
    /// </summary>
    private const string LogCategory = "[BudgetApproved]";
    
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
    /// <param name="category"></param>
    /// <param name="user">사용자 정보</param>
    /// <returns></returns>
    public async Task<Response> AddAsync(EnumDatabaseLogActionType actionType, string contents, string category,
        DbModelUser user)
    {
        Response result;
        
        try
        {
            // 로그 정보를 생성한다.
            DbModelLogAction add = new DbModelLogAction
            {
                Id = Guid.NewGuid() ,
                Contents = contents ,
                ActionType = actionType ,
                RegDate = DateTime.Now ,
                RegId = user.Id ,
                RegName = user.DisplayName ,
                Category = category ,
            };

            // 데이터베이스에 저장
            await _dbContext.LogActions.AddAsync(add);
            await _dbContext.SaveChangesAsync();
            
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