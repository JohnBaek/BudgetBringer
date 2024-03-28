using Microsoft.AspNetCore.Identity;
using Models.DataModels;

namespace Providers.Services;

/// <summary>
/// SignInManager<TUser> 래핑 클래스
/// </summary>
public interface ISignInService<in TUser> where TUser : User
{
    /// <summary>
    /// 사인 아웃
    /// </summary>
    /// <returns></returns>
    Task SignOutAsync();

    /// <summary>
    /// 유저정보로 강제로그인
    /// </summary>
    /// <param name="user">The user to sign-in.</param>
    /// <param name="isPersistent">Flag indicating whether the sign-in cookie should persist after the browser is closed.</param>
    /// <param name="authenticationMethod">Name of the method used to authenticate the user.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    Task SignInAsync(TUser user, bool isPersistent, string? authenticationMethod = null);
}