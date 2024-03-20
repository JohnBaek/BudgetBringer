using Features.Extensions;
using Microsoft.Extensions.Logging;
using Models.Common.Enums;
using Models.DataModels;
using Models.Requests.Login;
using Models.Responses;
using Providers.Repositories;
using Exception = System.Exception;

namespace Providers.Services;

/// <summary>
/// 로그인 서비스 구현체
/// </summary>
public class LoginService : ILoginService
{
    /// <summary>
    /// 로거
    /// </summary>
    private readonly ILogger<LoginService> _logger;

    /// <summary>
    /// 사용자 리파지토리
    /// </summary>
    private readonly IUserRepository _userRepository;
    
    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="logger">로거</param>
    /// <param name="userRepository">사용자 리파지토리</param>
    public LoginService( ILogger<LoginService> logger, IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
    }

    /// <summary>
    /// 로그인을 시도한다.
    /// </summary>
    /// <returns>결과</returns>
    public async Task<ResponseData<User>> TryLoginAsync(RequestLogin request)
    {
        ResponseData<User> result;

        try
        {
            // 요청이 유효하지 않은경우
            if(request.IsInValid())
                return new ResponseData<User>{ Code = "ERR", Message = request.GetFirstErrorMessage()};
            
            // 사용자를 찾지 못한경우 
            if(!await _userRepository.ExistUserAsync(request.LoginId))
                return new ResponseData<User>{ Code = "ERR", Message = "사용자를 찾지 못했습니다."};
            
            // 로그인을 시도한다.
            User? loginUser = await _userRepository.GetUserWithIdPasswordAsync(request.LoginId, request.Password.ToSha());
            
            // 로그인에 실패한경우
            if(loginUser == null)
                return new ResponseData<User>{ Code = "ERR", Message = "아이디 혹은 비밀번호가 다릅니다."};

            return new ResponseData<User>() {Result = EnumResponseResult.Success, Data = loginUser};
        }
        catch (Exception e)
        {
            result = new ResponseData<User>{Code = "ERR", Message = "처리중 예외가 발생했습니다." };
            e.LogError(_logger);
        }

        return result;
    }
}