using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DataModels;
using Models.Requests.Login;
using Models.Responses;
using Providers.Services;

namespace Apis.Controllers;

/// <summary>
/// 로그인 컨트롤러 
/// </summary>
[ApiController]
[AllowAnonymous]
[Route("api/v1/[controller]")]
public class LoginController : Controller
{
    /// <summary>
    /// 로그인 서비스
    /// </summary>
    private readonly IAuthenticationService _authenticationService;

    /// <summary>
    /// 사인인 서비스
    /// </summary>
    /// <returns></returns>
    private readonly ISignInService<User> _signInService;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="authenticationService">로그인 서비스</param>
    /// <param name="signInService">사인인 서비스</param>
    public LoginController(
          IAuthenticationService authenticationService
        , ISignInService<User> signInService) 
    {
        _authenticationService = authenticationService;
        _signInService = signInService;
    }
    
    
    /// <summary>
    /// 로그인을 시도한다.
    /// </summary>
    /// <param name="request">로그인 정보</param>
    /// <returns>로그인결과</returns>
    [HttpPost]
    public async Task<Response> TryLogin(RequestLogin request)
    {
        return await _authenticationService.TryLoginAsync(request);
    }
    
    
    /// <summary>
    /// 로그아웃을 처리한다.
    /// </summary>
    /// <returns></returns>
    [HttpGet("Logout")]
    public async Task<Response> Logout()
    {
        await _signInService.SignOutAsync();
        return new Response();
    }
    
    /// <summary>
    /// 로그인여부를 확인한다.
    /// </summary>
    /// <returns>로그인결과</returns>
    [HttpGet("IsAuthenticated")]
    public async Task<Response> IsAuthenticatedAsync()
    {
        return await _signInService.IsSignedIn(HttpContext);
    }
}