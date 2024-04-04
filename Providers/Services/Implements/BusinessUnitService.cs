using Features.Extensions;
using Microsoft.Extensions.Logging;
using Models.Common.Enums;
using Models.DataModels;
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
    /// 리파지토리
    /// </summary>
    private readonly IBusinessUnitRepository _repository;
    
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
        _repository = businessUnitRepository;
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
            response = await _repository.GetListAsync(requestQuery);
        }
        catch (Exception e)
        {
            response = new ResponseList<ResponseBusinessUnit>(EnumResponseResult.Error,"","처리중 예외가 발생했습니다.",null);
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
            response = await _repository.GetAsync(id);
        }
        catch (Exception e)
        {
            response = new ResponseData<ResponseBusinessUnit>(EnumResponseResult.Error,"","처리중 예외가 발생했습니다.",null);
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
    public async Task<Response> UpdateAsync(string id, RequestBusinessUnit request)
    {
        Response response;
        
        try
        {
            response = await _repository.UpdateAsync(id , request);
        }
        catch (Exception e)
        {
            response = new ResponseData<ResponseBusinessUnit>(EnumResponseResult.Error,"","처리중 예외가 발생했습니다.",null);
            e.LogError(_logger);
        }

        return response;
    }

    /// <summary>
    /// 데이터를 추가한다.
    /// </summary>
    /// <param name="request">요청정보</param>
    /// <returns></returns>
    public async Task<ResponseData<ResponseBusinessUnit>> AddAsync(RequestBusinessUnit request)
    {
        ResponseData<ResponseBusinessUnit> response;
        
        try
        {
            response = await _repository.AddAsync(request);
        }
        catch (Exception e)
        {
            response = new ResponseData<ResponseBusinessUnit>(EnumResponseResult.Error,"","처리중 예외가 발생했습니다.",null);
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
            response = new ResponseData<ResponseBusinessUnit>(EnumResponseResult.Error,"","처리중 예외가 발생했습니다.",null);
            e.LogError(_logger);
        }

        return response;
    }
}