using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DataModels;
using Models.Requests.Login;
using Models.Requests.Query;
using Models.Requests.Users;
using Models.Responses;
using Models.Responses.Users;
using Providers.Services.Interfaces;

namespace Apis.Controllers;


/// <summary>
/// 관리자 컨트롤러 
/// </summary>
[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/v1/[controller]")]
public class AdminController
{
    /// <summary>
    /// 로그인 서비스
    /// </summary>
    private readonly IAuthenticationService _authenticationService;

    /// <summary>
    /// 사인인 서비스
    /// </summary>
    /// <returns></returns>
    private readonly ISignInService<DbModelUser> _signInService;
    
    /// <summary>
    /// 유저 리파지토리
    /// </summary>
    private readonly IUserService _userService;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="authenticationService">로그인 서비스</param>
    /// <param name="signInService">사인인 서비스</param>
    /// <param name="userService"></param>
    public AdminController(
        IAuthenticationService authenticationService
        , ISignInService<DbModelUser> signInService
        , IUserService userService) 
    {
        _authenticationService = authenticationService;
        _signInService = signInService;
        _userService = userService;
    }
    
    /// <summary>
    /// 리스트를 가져온다.
    /// </summary>
    /// <param name="requestQuery">요청 정보</param>
    /// <returns></returns>
    [HttpGet("Users")]
    public async Task<ResponseList<ResponseUser>> GetListAsync([FromQuery] RequestQuery requestQuery)
    {
        return await _userService.GetListAsync(requestQuery);
    }
    
    /// <summary>
    /// 사용자의 패스워드를 변경한다.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("Users")]
    public async Task<Response> UpdateAsync(RequestUserChangePassword request)
    {
        return await _userService.UpdatePasswordAsync(request);
    }
}