using Microsoft.AspNetCore.Identity;
using Models.DataModels;

namespace Providers.Services;

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
}