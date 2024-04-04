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
/// 섹터 서비스 인터페이스
/// </summary>
public class SectorService : ISectorService
{
    /// <summary>
    /// 리파지토리
    /// </summary>
    private readonly ISectorRepository _repository;
    
    /// <summary>
    /// 로거
    /// </summary>
    private readonly ILogger<SectorService> _logger;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="sectorRepository">리파지토리</param>
    /// <param name="logger">로거</param>
    public SectorService(
        ISectorRepository sectorRepository
        , ILogger<SectorService> logger)
    {
        _repository = sectorRepository;
        _logger = logger;
    }
    /// <summary>
    /// 리스트를 가져온다.
    /// </summary>
    /// <param name="requestQuery">쿼리 정보</param>
    /// <returns></returns>
    public async Task<ResponseList<ResponseSector>> GetListAsync(RequestQuery requestQuery)
    {
        ResponseList<ResponseSector> response;
        
        try
        {
            response = await _repository.GetListAsync(requestQuery);
        }
        catch (Exception e)
        {
            response = new ResponseList<ResponseSector>(EnumResponseResult.Error,"","처리중 예외가 발생했습니다.",null);
            e.LogError(_logger);
        }

        return response;
    }

    /// <summary>
    /// 데이터를 가져온다.
    /// </summary>
    /// <param name="id">아이디</param>
    /// <returns></returns>
    public async Task<ResponseData<ResponseSector>> GetAsync(string id)
    {
        ResponseData<ResponseSector> response;
        
        try
        {
            response = await _repository.GetAsync(id);
        }
        catch (Exception e)
        {
            response = new ResponseData<ResponseSector>(EnumResponseResult.Error,"","처리중 예외가 발생했습니다.",null);
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
    public async Task<Response> UpdateAsync(string id, RequestSector request)
    {
        Response response;
        
        try
        {
            response = await _repository.UpdateAsync(id , request);
        }
        catch (Exception e)
        {
            response = new ResponseData<ResponseSector>(EnumResponseResult.Error,"","처리중 예외가 발생했습니다.",null);
            e.LogError(_logger);
        }

        return response;
    }

    /// <summary>
    /// 데이터를 추가한다.
    /// </summary>
    /// <param name="request">요청정보</param>
    /// <returns></returns>
    public async Task<ResponseData<ResponseSector>> AddAsync(RequestSector request)
    {
        ResponseData<ResponseSector> response;
        
        try
        {
            response = await _repository.AddAsync(request);
        }
        catch (Exception e)
        {
            response = new ResponseData<ResponseSector>(EnumResponseResult.Error,"","처리중 예외가 발생했습니다.",null);
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
            response = new ResponseData<ResponseSector>(EnumResponseResult.Error,"","처리중 예외가 발생했습니다.",null);
            e.LogError(_logger);
        }

        return response;
    }
}