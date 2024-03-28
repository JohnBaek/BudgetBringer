using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DataModels;
using Models.Requests.Login;
using Models.Responses;
using Models.Responses.Users;
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
    private readonly ILoginService _loginService;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="loginService">로그인 서비스</param>
    public LoginController(ILoginService loginService)
    {
        _loginService = loginService;
    }
    
    
    /// <summary>
    /// 로그인을 시도한다.
    /// </summary>
    /// <param name="request">로그인 정보</param>
    /// <returns>로그인결과</returns>
    [HttpPost]
    public async Task<ResponseData<ResponseUser>> TryLogin(RequestLogin request)
    {
        return await _loginService.TryLoginAsync(request);
    }
    
    /// <summary>
    /// 로그인을 시도한다.
    /// </summary>
    /// <param name="request">로그인 정보</param>
    /// <returns>로그인결과</returns>
    [HttpPost("/Test")]
    public async Task<ResponseData<ResponseUser>> Test(RequestLogin request)
    {
        return new ResponseData<ResponseUser>();
    }
}