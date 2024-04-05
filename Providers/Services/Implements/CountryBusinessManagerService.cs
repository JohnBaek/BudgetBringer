using Features.Extensions;
using Microsoft.Extensions.Logging;
using Models.Common.Enums;
using Models.Requests.Budgets;
using Models.Requests.Query;
using Models.Responses;
using Models.Responses.Budgets;
using Providers.Repositories.Interfaces;
using Providers.Services.Interfaces;

namespace Providers.Services.Implements;

/// <summary>
/// 컨트리 비지니스 매니저 (CBM) 서비스 인터페이스
/// </summary>
public class CountryBusinessManagerService : ICountryBusinessManagerService
{
    /// <summary>
    /// 리파지토리
    /// </summary>
    private readonly ICountryBusinessManagerRepository _repository;
    
    /// <summary>
    /// 로거
    /// </summary>
    private readonly ILogger<CountryBusinessManagerService> _logger;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="countryBusinessManagerRepository">리파지토리</param>
    /// <param name="logger">로거</param>
    public CountryBusinessManagerService(
        ICountryBusinessManagerRepository countryBusinessManagerRepository
        , ILogger<CountryBusinessManagerService> logger)
    {
        _repository = countryBusinessManagerRepository;
        _logger = logger;
    }
    
    /// <summary>
    /// 리스트를 가져온다.
    /// </summary>
    /// <param name="requestQuery">쿼리 정보</param>
    /// <returns></returns>
    public async Task<ResponseList<ResponseCountryBusinessManager>> GetListAsync(RequestQuery requestQuery)
    {
        ResponseList<ResponseCountryBusinessManager> response;
        
        try
        {
            response = await _repository.GetListAsync(requestQuery);
        }
        catch (Exception e)
        {
            response = new ResponseList<ResponseCountryBusinessManager>(EnumResponseResult.Error,"","처리중 예외가 발생했습니다.",null);
            e.LogError(_logger);
        }

        return response;
    }

    /// <summary>
    /// 데이터를 가져온다.
    /// </summary>
    /// <param name="id">아이디</param>
    /// <returns></returns>
    public async Task<ResponseData<ResponseCountryBusinessManager>> GetAsync(string id)
    {
        ResponseData<ResponseCountryBusinessManager> response;
        
        try
        {
            response = await _repository.GetAsync(id);
        }
        catch (Exception e)
        {
            response = new ResponseData<ResponseCountryBusinessManager>(EnumResponseResult.Error,"","처리중 예외가 발생했습니다.",null);
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
    public async Task<Response> UpdateAsync(string id, RequestCountryBusinessManager request)
    {
        Response response;
        
        try
        {
            response = await _repository.UpdateAsync(id , request);
        }
        catch (Exception e)
        {
            response = new ResponseData<ResponseCountryBusinessManager>(EnumResponseResult.Error,"","처리중 예외가 발생했습니다.",null);
            e.LogError(_logger);
        }

        return response;
    }

    /// <summary>
    /// 데이터를 추가한다.
    /// </summary>
    /// <param name="request">요청정보</param>
    /// <returns></returns>
    public async Task<ResponseData<ResponseCountryBusinessManager>> AddAsync(RequestCountryBusinessManager request)
    {
        ResponseData<ResponseCountryBusinessManager> response;
        
        try
        {
            response = await _repository.AddAsync(request);
        }
        catch (Exception e)
        {
            response = new ResponseData<ResponseCountryBusinessManager>(EnumResponseResult.Error,"","처리중 예외가 발생했습니다.",null);
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
            response = new ResponseData<ResponseCountryBusinessManager>(EnumResponseResult.Error,"","처리중 예외가 발생했습니다.",null);
            e.LogError(_logger);
        }

        return response;
    }

    /// <summary>
    /// 매니저에 비지니스 유닛을 추가한다.
    /// </summary>
    /// <param name="managerId"></param>
    /// <param name="unitId"></param>
    /// <returns></returns>
    public async Task<ResponseData<ResponseCountryBusinessManager>> AddUnitAsync(string managerId, string unitId)
    {
        ResponseData<ResponseCountryBusinessManager> response;
        
        try
        {
            response = await _repository.AddUnitAsync(managerId,unitId);
        }
        catch (Exception e)
        {
            response = new ResponseData<ResponseCountryBusinessManager>(EnumResponseResult.Error,"","처리중 예외가 발생했습니다.",null);
            e.LogError(_logger);
        }

        return response;
    }

    /// <summary>
    /// 매니저에 비지니스 유닛을 제거한다..
    /// </summary>
    /// <param name="managerId"></param>
    /// <param name="unitId"></param>
    /// <returns></returns>
    public async Task<Response> DeleteUnitAsync(string managerId, string unitId)
    {
        Response response;
        
        try
        {
            response = await _repository.DeleteUnitAsync(managerId,unitId);
        }
        catch (Exception e)
        {
            response = new ResponseData<ResponseCountryBusinessManager>(EnumResponseResult.Error,"","처리중 예외가 발생했습니다.",null);
            e.LogError(_logger);
        }

        return response;
    }
}