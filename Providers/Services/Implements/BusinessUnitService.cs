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
/// 비지니스유닛 (BU) 서비스 
/// </summary>
public class BusinessUnitService : IBusinessUnitService
{
    /// <summary>
    /// 비지니스 유닛 Repository
    /// </summary>
    private readonly IBusinessUnitRepository _businessUnitRepository;
    
    /// <summary>
    /// 로거
    /// </summary>
    private readonly ILogger<BusinessUnitService> _logger;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="businessUnitRepository">비지니스 유닛 Repository</param>
    /// <param name="logger">로거</param>
    public BusinessUnitService(
          IBusinessUnitRepository businessUnitRepository
        , ILogger<BusinessUnitService> logger)
    {
        _businessUnitRepository = businessUnitRepository;
        _logger = logger;
    }

    /// <summary>
    /// 리스트를 가져온다.
    /// </summary>
    /// <param name="requestQuery">쿼리 정보</param>
    /// <returns></returns>
    public async Task<ResponseList<ResponseBusinessUnit>> GetListAsync(RequestQuery requestQuery)
    {
        ResponseList<ResponseBusinessUnit> response;
        
        try
        {
            response = new ResponseList<ResponseBusinessUnit>();
        }
        catch (Exception e)
        {
            response = new ResponseList<ResponseBusinessUnit>("처리중 예외가 발생했습니다.");
            e.LogError(_logger);
        }

        return response;
    }

    /// <summary>
    /// 데이터를 가져온다.
    /// </summary>
    /// <param name="id">아이디</param>
    /// <returns></returns>
    public async Task<ResponseData<ResponseBusinessUnit>> GetAsync(string id)
    {
        ResponseData<ResponseBusinessUnit> response;
        
        try
        {
            response = new ResponseData<ResponseBusinessUnit>();
        }
        catch (Exception e)
        {
            response = new ResponseData<ResponseBusinessUnit>("처리중 예외가 발생했습니다.");
            e.LogError(_logger);
        }

        return response;
    }

    /// <summary>
    /// 데이터를 업데이트한다.
    /// </summary>
    /// <param name="request">요청정보</param>
    /// <returns></returns>
    public async Task<Response> UpdateAsync(RequestBusinessUnit request)
    {
        ResponseData<ResponseBusinessUnit> response;
        
        try
        {
            response = new ResponseData<ResponseBusinessUnit>();
        }
        catch (Exception e)
        {
            response = new ResponseData<ResponseBusinessUnit>("처리중 예외가 발생했습니다.");
            e.LogError(_logger);
        }

        return response;
    }

    /// <summary>
    /// 데이터를 추가한다.
    /// </summary>
    /// <param name="request">요청정보</param>
    /// <returns></returns>
    public async Task<Response> AddAsync(RequestBusinessUnit request)
    {
        ResponseData<ResponseBusinessUnit> response;
        
        try
        {
            response = new ResponseData<ResponseBusinessUnit>();
        }
        catch (Exception e)
        {
            response = new ResponseData<ResponseBusinessUnit>("처리중 예외가 발생했습니다.");
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
        ResponseData<ResponseBusinessUnit> response;
        
        try
        {
            response = new ResponseData<ResponseBusinessUnit>();
        }
        catch (Exception e)
        {
            response = new ResponseData<ResponseBusinessUnit>("처리중 예외가 발생했습니다.");
            e.LogError(_logger);
        }

        return response;
    }
}