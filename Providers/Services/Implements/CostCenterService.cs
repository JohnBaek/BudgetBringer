using Features.Extensions;
using Microsoft.Extensions.Logging;
using Models.Requests.Budgets;
using Models.Requests.Query;
using Models.Responses;
using Models.Responses.Budgets;
using Providers.Repositories.Interfaces;
using Providers.Services.Interfaces;

namespace Providers.Services.Implements;

/// <summary>
/// 코스트센터 서비스 인터페이스
/// </summary>
public class CostCenterService : ICostCenterService
{
    /// <summary>
    /// 리파지토리
    /// </summary>
    private readonly ICostCenterRepository _repository;
    
    /// <summary>
    /// 로거
    /// </summary>
    private readonly ILogger<CostCenterService> _logger;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="costCenterRepository">리파지토리</param>
    /// <param name="logger">로거</param>
    public CostCenterService(
        ICostCenterRepository costCenterRepository
        , ILogger<CostCenterService> logger)
    {
        _repository = costCenterRepository;
        _logger = logger;
    }
    /// <summary>
    /// 리스트를 가져온다.
    /// </summary>
    /// <param name="requestQuery">쿼리 정보</param>
    /// <returns></returns>
    public async Task<ResponseList<ResponseCostCenter>> GetListAsync(RequestQuery requestQuery)
    {
        ResponseList<ResponseCostCenter> response;
        
        try
        {
            response = await _repository.GetListAsync(requestQuery);
        }
        catch (Exception e)
        {
            response = new ResponseList<ResponseCostCenter>("처리중 예외가 발생했습니다.");
            e.LogError(_logger);
        }

        return response;
    }

    /// <summary>
    /// 데이터를 가져온다.
    /// </summary>
    /// <param name="id">아이디</param>
    /// <returns></returns>
    public async Task<ResponseData<ResponseCostCenter>> GetAsync(string id)
    {
        ResponseData<ResponseCostCenter> response;
        
        try
        {
            response = await _repository.GetAsync(id);
        }
        catch (Exception e)
        {
            response = new ResponseData<ResponseCostCenter>("처리중 예외가 발생했습니다.");
            e.LogError(_logger);
        }

        return response;
    }

    /// <summary>
    /// 데이터를 업데이트한다.
    /// </summary>
    /// <param name="id">아이디 값</param>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Response> UpdateAsync(string id, RequestCostCenter request)
    {
        Response response;
        
        try
        {
            response = await _repository.UpdateAsync(id , request);
        }
        catch (Exception e)
        {
            response = new ResponseData<ResponseCostCenter>("처리중 예외가 발생했습니다.");
            e.LogError(_logger);
        }

        return response;
    }

    /// <summary>
    /// 데이터를 추가한다.
    /// </summary>
    /// <param name="request">요청정보</param>
    /// <returns></returns>
    public async Task<ResponseData<ResponseCostCenter>> AddAsync(RequestCostCenter request)
    {
        ResponseData<ResponseCostCenter> response;
        
        try
        {
            response = await _repository.AddAsync(request);
        }
        catch (Exception e)
        {
            response = new ResponseData<ResponseCostCenter>("처리중 예외가 발생했습니다.");
            e.LogError(_logger);
        }

        return response;
    }

    /// <summary>
    /// 데이터를 삭제한다.
    /// </summary>
    /// <param name="id">대상 아이디값</param>
    /// <returns></returns>
    public async Task<Response> DeleteAsync(string id)
    {
        Response response;
        
        try
        {
            response = await _repository.DeleteAsync(id);
        }
        catch (Exception e)
        {
            response = new ResponseData<ResponseCostCenter>("처리중 예외가 발생했습니다.");
            e.LogError(_logger);
        }

        return response;
    }
}