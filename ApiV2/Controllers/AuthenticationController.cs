using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DataModels;
using Models.Requests.Login;
using Models.Responses;
using Models.Responses.Users;
using Providers.Services.Interfaces;

namespace Apis.Controllers;

/// <summary>
/// 로그인 컨트롤러 
/// </summary>
[ApiController]
[AllowAnonymous]
[Route("api/v2/[controller]")]
public class AuthenticationController
{
    /// <summary>
    /// 로그인을 시도한다.
    /// </summary>
    /// <param name="request">로그인 정보</param>
    /// <returns>로그인결과</returns>
    [HttpPost("Login")]
    public async Task<ResponseData<ResponseUser>> TryLogin(RequestLogin request)
    {
        return new ResponseData<ResponseUser>();
    }


    [HttpGet("test")]
    public string Test()
    {
        return "Test";
    }
}