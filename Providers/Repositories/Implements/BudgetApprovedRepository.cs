using Features.Extensions;
using Microsoft.Extensions.Logging;
using Models.DataModels;
using Models.Requests.Budgets;
using Models.Requests.Query;
using Models.Responses;
using Providers.Repositories.Interfaces;

namespace Providers.Repositories.Implements;

/// <summary>
/// 예산 승인 리파지토리
/// </summary>
public class BudgetApprovedRepository : IBudgetApprovedRepository
{
    /// <summary>
    /// DB Context
    /// </summary>
    private readonly AnalysisDbContext _dbContext;

    /// <summary>
    /// 로거
    /// </summary>
    private readonly ILogger<BudgetApprovedRepository> _logger;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="logger"></param>
    public BudgetApprovedRepository(AnalysisDbContext dbContext, ILogger<BudgetApprovedRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }


    /// <summary>
    /// 리스트를 가져온다.
    /// </summary>
    /// <param name="requestQuery">쿼리 정보</param>
    /// <returns></returns>
    public async Task<List<DbModelBudgetApproved>> GetListAsync(RequestQuery requestQuery)
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