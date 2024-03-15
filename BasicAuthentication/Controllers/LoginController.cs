using BasicAuthenticated.Resources;
using BasicAuthentication.Interfaces;
using BasicAuthentication.ViewModels.Requests.Login;
using BasicAuthentication.ViewModels.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BasicAuthentication.Controllers;

/// <summary>
/// 로그인 컨트롤러
/// </summary>
[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    /// <summary>
    /// 로그인 서비스 프로바이더
    /// </summary>
    private readonly ILoginService _loginService;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="loginService">로그인 서비스 프로바이더</param>
    public LoginController(ILoginService loginService)
    {
        _loginService = loginService;
    }

    /// <summary>
    /// 로그인
    /// </summary>
    /// <param name="request">로그인 요청 정보</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<Response> Login([FromBody] RequestLogin request)
    {
        return await _loginService.TryLogin(request);
    }
}