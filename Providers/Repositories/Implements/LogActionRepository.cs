using System.Linq.Expressions;
using Features.Extensions;
using Microsoft.Extensions.Logging;
using Models.Common.Enums;
using Models.DataModels;
using Models.Requests.Query;
using Models.Responses;
using Models.Responses.Logs;
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
    /// 쿼리 서비스
    /// </summary>
    private readonly IQueryService _queryService;


    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="logger">로거</param>
    /// <param name="dbContext">디비컨텍스트</param>
    /// <param name="queryService">쿼리서비스</param>
    public LogActionRepository(ILogger<LogActionRepository> logger, AnalysisDbContext dbContext, IQueryService queryService)
    {
        _logger = logger;
        _dbContext = dbContext;
        _queryService = queryService;
    }
    
        
    /// <summary>
    /// 셀렉터 매핑 정의
    /// </summary>
    private Expression<Func<DbModelLogAction, ResponseLogAction>> MapDataToResponse { get; init; } = item => new ResponseLogAction
    {
        RegDate = item.RegDate,
        RegName = item.RegName ?? "",
        Contents = item.Contents,
        Category = item.Category,
        ActionType = item.ActionType,
    };


    /// <summary>
    /// 리스트를 가져온다.
    /// </summary>
    /// <param name="requestQuery">쿼리 정보</param>
    /// <returns></returns>
    public async Task<ResponseList<ResponseLogAction>> GetListAsync(RequestQuery requestQuery)
    {
        ResponseList<ResponseLogAction> result = new ResponseList<ResponseLogAction>();
        try
        {
            // 기본 Sort가 없을 경우 
            if (requestQuery.SortOrders is { Count: 0 })
            {
                requestQuery.SortOrders.Add("Desc");
                requestQuery.SortFields?.Add(nameof(ResponseLogAction.RegDate));
            }
            
            // 검색 메타정보 추가
            requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains ,nameof(ResponseLogAction.RegDate));
            requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains ,nameof(ResponseLogAction.RegName));
            requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains ,nameof(ResponseLogAction.Contents));
            requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains ,nameof(ResponseLogAction.Category));
            requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains ,nameof(ResponseLogAction.ActionType));
            
            // 결과를 반환한다.
            result = await _queryService.ToResponseListAsync(requestQuery, MapDataToResponse);
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