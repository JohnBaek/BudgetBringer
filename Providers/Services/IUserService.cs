using Microsoft.AspNetCore.Http;
using Models.Responses;

namespace Providers.Services;

/// <summary>
/// 유저 서비스 인터페이스
/// </summary>
public interface IUserService
{
    /// <summary>
    /// 로그인한 사용자의 권한 목록을 가져온다.
    /// </summary>
    /// <param name="httpContext">HttpContext</param>
    /// <returns></returns>
    Task<ResponseList<string>> GetRolesByUserAsync(HttpContext httpContext);
}