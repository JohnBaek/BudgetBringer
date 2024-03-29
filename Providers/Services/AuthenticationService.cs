using Features.Extensions;
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
/// 로그인 서비스 구현체
/// </summary>
public class AuthenticationService : IAuthenticationService
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
    /// 생성자
    /// </summary>
    /// <param name="logger">로거</param>
    /// <param name="userRepository">사용자 리파지토리</param>
    /// <param name="signInService"></param>
    public AuthenticationService( ILogger<AuthenticationService> logger, IUserRepository userRepository, ISignInService<User> signInService)
    {
        _logger = logger;
        _userRepository = userRepository;
        _signInService = signInService;
    }
    
    /// <summary>
    /// 로그인을 시도한다.
    /// </summary>
    /// <returns>결과</returns>
    public async Task<ResponseData<ResponseUser>> TryLoginAsync(RequestLogin request)
    {
        ResponseData<ResponseUser> result;
    
        try
        {
            Console.WriteLine(request.Password.ToSHA());
            
            // 요청이 유효하지 않은경우
            if(request.IsInValid())
                return new ResponseData<ResponseUser>{ Code = "ERR", Message = request.GetFirstErrorMessage()};
            
            // 사용자를 찾지 못한경우 
            if(!await _userRepository.ExistUserAsync(request.LoginId))
                return new ResponseData<ResponseUser>{ Code = "ERR", Message = "사용자를 찾지 못했습니다."};
            
            // 로그인을 시도한다.
            User? loginUser = await _userRepository.GetUserWithIdPasswordAsync(request.LoginId, request.Password);
            
            // 아이디 패스워드 인증에 실패한경우
            if(loginUser == null)
                return new ResponseData<ResponseUser>{ Code = "ERR", Message = "아이디 혹은 비밀번호가 다릅니다."};

            // 로그인 시킨다.
            await _signInService.SignInAsync(loginUser, isPersistent: true);
            return new ResponseData<ResponseUser>
            {
                Result = EnumResponseResult.Success , 
                Data = new ResponseUser
                {
                    Name = loginUser.UserName ?? "",
                    Roles = await _userRepository.GetRolesByUserAsync(loginUser.Id)
                }
            };
        }
        catch (Exception e)
        {
            result = new ResponseData<ResponseUser>{Code = "ERR", Message = "처리중 예외가 발생했습니다." };
            e.LogError(_logger);
        }
    
        return result;
    }
}