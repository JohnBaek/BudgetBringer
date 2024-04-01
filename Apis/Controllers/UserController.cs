using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DataModels;
using Models.Responses;
using Providers.Services;

namespace Apis.Controllers;


/// <summary>
/// 유저 컨트롤러 
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class UserController : Controller
{
    /// <summary>
    /// 로그인 서비스
    /// </summary>
    private readonly IAuthenticationService _authenticationService;

    /// <summary>
    /// 유저 리파지토리
    /// </summary>
    private readonly IUserService _userService;

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
    /// <param name="userService">유저 서비스</param>
    public UserController(
        IAuthenticationService authenticationService
        , ISignInService<User> signInService, IUserService userService) 
    {
        _authenticationService = authenticationService;
        _signInService = signInService;
        _userService = userService;
    }
    
    
    /// <summary>
    /// 로그인한 사용자의 역할 정보를 가져온다.
    /// </summary>
    /// <returns>로그인결과</returns>
    [HttpGet("Roles")]
    public async Task<ResponseList<string>> GetUserRolesAsync()
    {
        return await _userService.GetRolesByUserAsync(this.HttpContext);
    }
}