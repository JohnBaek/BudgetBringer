using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Models.Common.Enums;
using Models.DataModels;
using Models.Responses;
using Providers.Services.Interfaces;

namespace Providers.Services.Implements;

/// <summary>
/// SignInManager 래핑 클래스 구현체
/// </summary>
public class SignInService : ISignInService<DbModelUser>
{
    /// <summary>
    /// 사인인 매니저
    /// </summary>
    private readonly SignInManager<DbModelUser> _signInManager;
    
    /// <summary>
    /// IHttpContextAccessor
    /// </summary>
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="signInManager">사인인 매니저</param>
    /// <param name="httpContextAccessor">IHttpContextAccessor</param>
    public SignInService(SignInManager<DbModelUser> signInManager, IHttpContextAccessor httpContextAccessor)
    {
        _signInManager = signInManager;
        _httpContextAccessor = httpContextAccessor;
    }


    /// <summary>
    /// 사인 아웃
    /// </summary>
    /// <returns></returns>
    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    /// <summary>
    /// 유저정보로 강제로그인
    /// </summary>
    /// <param name="dbModelUser">The dbModelUser to sign-in.</param>
    /// <param name="isPersistent">Flag indicating whether the sign-in cookie should persist after the browser is closed.</param>
    /// <param name="authenticationMethod">DisplayName of the method used to authenticate the dbModelUser.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task SignInAsync(DbModelUser dbModelUser, bool isPersistent, string? authenticationMethod = null)
    {
        await _signInManager.SignInAsync(dbModelUser, isPersistent, authenticationMethod);
    }

    /// <summary>
    /// 로그인여부를 반환한다.
    /// </summary>
    /// <returns></returns>
    public Response IsSignedIn()
    {
        Response response = new Response{ IsAuthenticated = false };

        // 세션이 비어있을 경우
        if (_httpContextAccessor.HttpContext?.User == null)
            return response;
        
        // 로그인 여부를 가져온다.
        bool isAuthenticated = _signInManager.IsSignedIn(_httpContextAccessor.HttpContext.User);

        // 인증되어있는 경우 
        if (isAuthenticated)
            return new Response {Result = EnumResponseResult.Success, IsAuthenticated = true};
        
        return response;
    }

    /// <summary>
    /// 패스워드 정보로 로그인 시킨다.
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <param name="isPersistent"></param>
    /// <param name="lockoutOnFailure"></param>
    /// <returns></returns>
    public async Task<Response> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
    {
        Response response;

        // 로그인을 시도한다.
        SignInResult loginResult = await _signInManager.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);

        // 성공한경우  
        if (loginResult.Succeeded)
            return new Response(EnumResponseResult.Success,"","");
        
        // 계정이 잠긴경우 
        if(loginResult.IsLockedOut)
            return new Response(EnumResponseResult.Error,"", "계정이 잠겨있습니다.");
        
        // 허용되지 않는경우 
        if(loginResult.IsNotAllowed)
           return new Response(EnumResponseResult.Error, "허용되지 않습니다.","");

        // 실패한경우 
        return loginResult == SignInResult.Failed
            ? new Response(EnumResponseResult.Error, "아이디 또는 비밀번호를 확인해주세요", "")
            : new Response(EnumResponseResult.Error, "예외가 발생했습니다.", "");
    }
}