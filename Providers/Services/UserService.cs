using Features.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Models.Common.Enums;
using Models.DataModels;
using Models.Requests.Login;
using Models.Responses;
using Models.Responses.Users;
using Providers.Repositories;
using Exception = System.Exception;

namespace Providers.Services;

/// <summary>
/// 사용자 서비스 구현체
/// </summary>
public class UserService : IUserService
{
    /// <summary>
    /// 로거
    /// </summary>
    private readonly ILogger<AuthenticationService> _logger;
    
    /// <summary>
    /// 사용자 리파지토리
    /// </summary>
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// 사인인 매니저 
    /// </summary>
    private readonly ISignInService<User> _signInService;

    /// <summary>
    /// 유저 매니저
    /// </summary>
    private readonly UserManager<User> _userManager;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="logger">로거</param>
    /// <param name="userRepository">사용자 리파지토리</param>
    /// <param name="signInService"></param>
    /// <param name="userManager"></param>
    public UserService( 
        ILogger<AuthenticationService> logger
        , IUserRepository userRepository
        , ISignInService<User> signInService
        , UserManager<User> userManager)
    {
        _logger = logger;
        _userRepository = userRepository;
        _signInService = signInService;
        _userManager = userManager;
    }
    
    /// <summary>
    /// 로그인한 사용자의 권한 목록을 가져온다.
    /// </summary>
    /// <returns></returns>
    public async Task<ResponseList<string>> GetRolesByUserAsync(HttpContext httpContext)
    {
        ResponseList<string> result = new ResponseList<string>();
    
        try
        {
            User? user = await _userManager.GetUserAsync(httpContext.User);

            // 세션에 사용자 정보가 없는경우 
            if (user == null)
                return result;

            result.Items = await _userRepository.GetRolesByUserAsync(user.Id);
        }
        catch (Exception e)
        {
            result = new ResponseList<string>{ Code = "ERR", Message = "처리중 예외가 발생했습니다." };
            e.LogError(_logger);
        }
    
        return result;
    }
}