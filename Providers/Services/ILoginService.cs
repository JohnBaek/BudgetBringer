using Models.DataModels;
using Models.Requests.Login;
using Models.Responses;
using Models.Responses.Users;

namespace Providers.Services;

/// <summary>
/// 로그인 서비스 인터페이스 클래스
/// </summary>
public interface ILoginService
{
    /// <summary>
    /// 로그인을 시도한다.
    /// </summary>
    /// <returns>결과</returns>
    Task<ResponseData<ResponseUser>> TryLoginAsync(RequestLogin request);
}