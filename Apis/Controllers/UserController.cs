using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Responses;
using Models.Responses.Users;
using Providers.Services.Interfaces;

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
    /// 유저 리파지토리
    /// </summary>
    private readonly IUserService _userService;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="userService">유저 서비스</param>
    public UserController(
          IUserService userService) 
    {
        _userService = userService;
    }
    
    /// <summary>
    /// 로그인한 사용자의 Claim 정보를 가져온다.
    /// </summary>
    /// <returns>로그인결과</returns>
    [HttpGet("Roles")]
    public async Task<ResponseList<ResponseUserRole>> GetUserClaimsAsync()
    {
        return await _userService.GetRolesByUserAsync();
    }
    
    /// <summary>
    /// 로그인한 사용자의 정보를 가져온다.
    /// </summary>
    /// <returns>로그인결과</returns>
    [HttpGet("")]
    public async Task<ResponseData<ResponseUser>> GetUserAsync()
    {
        return await _userService.GetUserAsync();
    }
}