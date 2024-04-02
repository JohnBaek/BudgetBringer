
using Features.Extensions;
using Microsoft.Extensions.Logging;
using Models.DataModels;
using Models.Requests.Budgets;
using Models.Requests.Query;
using Models.Responses;
using Providers.Repositories.Interfaces;

namespace Providers.Repositories.Implements;

/// <summary>
/// 섹터 리파지토리 구현체
/// </summary>
public class SectorRepository : ISectorRepository
{
    /// <summary>
    /// DB Context
    /// </summary>
    private readonly AnalysisDbContext _dbContext;

    /// <summary>
    /// 로거
    /// </summary>
    private readonly ILogger<SectorRepository> _logger;
    
    /// <summary>
    /// 로그액션 리파지토리
    /// </summary>
    private readonly ILogActionRepository _logActionRepository;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="logger">로거</param>
    /// <param name="dbContext">디비컨텍스트</param>
    /// <param name="logActionRepository"></param>
    public SectorRepository(ILogger<SectorRepository> logger, AnalysisDbContext dbContext, ILogActionRepository logActionRepository)
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
    public async Task<List<DbModelSector>> GetListAsync(RequestQuery requestQuery)
    {
        throw new NotImplementedException();
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