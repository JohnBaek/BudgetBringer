using Models.Requests.Login;
using Models.Responses;
using Models.Responses.Authentication;
using Models.Responses.Users;

namespace Providers.Services.Interfaces;

/// <summary>
/// 로그인 서비스 인터페이스 클래스
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// 로그인을 시도한다.
    /// </summary>
    /// <returns>결과</returns>
    Task<ResponseData<ResponseUser>> TryLoginAsync(RequestLogin request);

    /// <summary>
    /// 로그인을 시도한다.
    /// </summary>
    /// <returns>결과</returns>
    Task<ResponseData<ResponseToken>> TryLoginForTokenAsync(RequestLogin request);
}