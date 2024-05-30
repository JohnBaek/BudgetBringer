using Features.Extensions;
using Microsoft.Extensions.Logging;
using Models.Common.Enums;
using Models.DataModels;
using Models.Requests.Login;
using Models.Responses;
using Models.Responses.Authentication;
using Models.Responses.Users;
using Providers.Repositories.Interfaces;
using Providers.Services.Interfaces;
using Exception = System.Exception;

namespace Providers.Services.Implements;

/// <summary>
/// 로그인 서비스 구현체
/// </summary>
public class AuthenticationService : IAuthenticationService
{
    /// <summary>
    /// Logger
    /// </summary>
    private readonly ILogger<AuthenticationService> _logger;
    
    /// <summary>
    /// 사용자 리파지토리
    /// </summary>
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// 사인인 매니저 
    /// </summary>
    private readonly ISignInService<DbModelUser> _signInService;
    
    /// <summary>
    /// 유저 리파지토리
    /// </summary>
    private readonly IUserService _userService;

    /// <summary>
    /// _jwtTokenService
    /// </summary>
    private readonly IJwtTokenService _jwtTokenService;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="logger">Logger</param>
    /// <param name="userRepository">사용자 리파지토리</param>
    /// <param name="signInService"></param>
    /// <param name="userService"></param>
    /// <param name="jwtTokenService"></param>
    public AuthenticationService( ILogger<AuthenticationService> logger, IUserRepository userRepository, ISignInService<DbModelUser> signInService, IUserService userService, IJwtTokenService jwtTokenService)
    {
        _logger = logger;
        _userRepository = userRepository;
        _signInService = signInService;
        _userService = userService;
        _jwtTokenService = jwtTokenService;
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
            // 요청이 유효하지 않은경우
            if(request.IsInValid())
                return new ResponseData<ResponseUser>{ Code = "ERR", Message = request.GetFirstErrorMessage()};
            
            // 사용자를 찾지 못한경우 
            if(!await _userRepository.ExistUserAsync(request.LoginId))
                return new ResponseData<ResponseUser>{ Code = "ERR", Message = "사용자를 찾지 못했습니다."};
            
            // 해당 정보로 로그인을 시도한다.
            Response loginResult = await _signInService.PasswordSignInAsync(request.LoginId, request.Password, isPersistent:false , lockoutOnFailure:false);
          
            // 실패한경우 
            if (loginResult.Result != EnumResponseResult.Success)
                return new ResponseData<ResponseUser>
                {
                    IsAuthenticated = false,
                    Code = loginResult.Code,
                    Data = null,
                    Message = loginResult.Message
                };
            
            // 사용자를 가져온다.
            DbModelUser? loginUser = await _userRepository.GetUserWithIdPasswordAsync(request.LoginId, request.Password);
            
            // 사용자가 없는경우
            if(loginUser == null)
                return  new ResponseData<ResponseUser>
                {
                    IsAuthenticated = false,
                    Code = loginResult.Code,
                    Data = null,
                    Message = loginResult.Message
                };

            // 성공한 경우 
            return new ResponseData<ResponseUser>
            {
                Result = EnumResponseResult.Success ,
                IsAuthenticated = true,
                Code = loginResult.Code,
                Data = new ResponseUser{ DisplayName = loginUser.DisplayName , Roles = (await _userService.GetRolesByUserAsync()).Items! },
                Message = loginResult.Message
            };
        }
        catch (Exception e)
        {
            result = new ResponseData<ResponseUser>{Code = "ERR", Message = "처리중 예외가 발생했습니다." };
            e.LogError(_logger);
        }
    
        return result;
    }

    /// <summary>
    /// Token
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<ResponseData<ResponseToken>> TryLoginForTokenAsync(RequestLogin request)
    {
        ResponseData<ResponseToken> result;
        try
        {
            // // Try Login to server
            // ResponseData<ResponseUser> loginResult = await TryLoginAsync(request: request);
            //
            // // If Not Success
            // if (!loginResult.Success)
            //     return new ResponseData<ResponseToken>(loginResult.Result, loginResult.Code, loginResult.Message, null);

            // Get Token and Refresh Token
            result = await _jwtTokenService.GenerateAsync(request.LoginId, 20);
        }
        catch (Exception e)
        {
            result = new ResponseData<ResponseToken>{Code = "ERR", Message = "처리중 예외가 발생했습니다." };
            e.LogError(_logger);
        }

        return result;
    }
}