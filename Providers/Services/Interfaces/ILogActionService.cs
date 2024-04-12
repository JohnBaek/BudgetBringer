using Models.Requests.Query;
using Models.Responses;
using Models.Responses.Logs;

namespace Providers.Services.Interfaces;

/// <summary>
/// 액션 로그 모델 인터페이스
/// </summary>
public interface ILogActionService
{
    /// <summary>
    /// 리스트를 가져온다.
    /// </summary>
    /// <param name="requestQuery">쿼리 정보</param>
    /// <returns></returns>
    Task<ResponseList<ResponseLogAction>> GetListAsync(RequestQuery requestQuery);
}