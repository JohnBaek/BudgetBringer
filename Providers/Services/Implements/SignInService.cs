using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Models.Common.Enums;
using Models.DataModels;
using Models.Responses;

namespace Providers.Services.Implements;

/// <summary>
/// SignInManager<TUser> 래핑 클래스 구현체
/// </summary>
public class SignInService : ISignInService<User>
{
    /// <summary>
    /// 사인인 매니저
    /// </summary>
    private readonly SignInManager<User> _signInManager;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="signInManager">사인인 매니저</param>
    public SignInService(SignInManager<User> signInManager)
    {
        _signInManager = signInManager;
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
    /// <param name="user">The user to sign-in.</param>
    /// <param name="isPersistent">Flag indicating whether the sign-in cookie should persist after the browser is closed.</param>
    /// <param name="authenticationMethod">Name of the method used to authenticate the user.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task SignInAsync(User user, bool isPersistent, string? authenticationMethod = null)
    {
        await _signInManager.SignInAsync(user, isPersistent, authenticationMethod);
    }

    /// <summary>
    /// 로그인여부를 반환한다.
    /// </summary>
    /// <param name="httpContext">ClaimsPrincipal</param>
    /// <returns></returns>
    public async Task<Response> IsSignedIn(HttpContext httpContext)
    {
        Response response = new Response{ IsAuthenticated = false };
        
        // 로그인 여부를 가져온다.
        bool isAuthenticated = _signInManager.IsSignedIn(httpContext.User);

        // 인증되어있는 경우 
        if (isAuthenticated)
            return new Response {Result = EnumResponseResult.Success, IsAuthenticated = true};
        
        // 인증되어있지 않은경우
        await _signInManager.SignOutAsync();
        
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
            return new Response("", "", true, EnumResponseResult.Success);
        
        // 계정이 잠긴경우 
        if(loginResult.IsLockedOut)
            return new Response("", "계정이 잠겨있습니다.", false, EnumResponseResult.Error);
        
        // 허용되지 않는경우 
        if(loginResult.IsNotAllowed)
            return new Response("", "허용되지 않습니다.", false, EnumResponseResult.Error);
        
        // 실패한경우 
        if(loginResult == SignInResult.Failed)
            return new Response("", "아이디 또는 비밀번호를 확인해주세요", false, EnumResponseResult.Error);
            
        
        return new Response("", "예외가 발생했습니다.", false, EnumResponseResult.Error);
    }
}