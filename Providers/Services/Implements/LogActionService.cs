using Features.Extensions;
using Microsoft.Extensions.Logging;
using Models.Common.Enums;
using Models.Requests.Query;
using Models.Responses;
using Models.Responses.Logs;
using Providers.Repositories.Interfaces;
using Providers.Services.Interfaces;

namespace Providers.Services.Implements;

/// <summary>
/// 액션 로그 모델  서비스 
/// </summary>
public class LogActionService : ILogActionService
{
    /// <summary>
    /// 리파지토리
    /// </summary>
    private readonly ILogActionRepository _repository;
    
    /// <summary>
    /// 로거
    /// </summary>
    private readonly ILogger<LogActionService> _logger;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="repository">X Repository</param>
    /// <param name="logger">로거</param>
    public LogActionService(
          ILogActionRepository repository
        , ILogger<LogActionService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// 리스트를 가져온다.
    /// </summary>
    /// <param name="requestQuery">쿼리 정보</param>
    /// <returns></returns>
    public async Task<ResponseList<ResponseLogAction>> GetListAsync(RequestQuery requestQuery)
    {
        ResponseList<ResponseLogAction> response;
        
        try
        {
            response = await _repository.GetListAsync(requestQuery);
        }
        catch (Exception e)
        {
            response = new ResponseList<ResponseLogAction>(EnumResponseResult.Error,"","처리중 예외가 발생했습니다.",null);
            e.LogError(_logger);
        }

        return response;
    }
}