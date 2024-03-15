using BasicAuthentication.ViewModels.Requests.Login;
using BasicAuthentication.ViewModels.Responses;

namespace BasicAuthentication.Interfaces;

/// <summary>
/// 로그인 인터페이스
/// </summary>
public interface ILoginService
{
    /// <summary>
    /// 로그인을 시도한다.
    /// </summary>
    /// <param name="request">로그인 요청정보</param>
    /// <returns>결과</returns>
    Task<Response> TryLogin(RequestLogin request);
}