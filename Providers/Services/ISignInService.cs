using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Models.DataModels;
using Models.Responses;

namespace Providers.Services;

/// <summary>
/// SignInManager<TUser> 래핑 클래스
/// </summary>
public interface ISignInService<in TUser> where TUser : DbModelUser
{
    /// <summary>
    /// 사인 아웃
    /// </summary>
    /// <returns></returns>
    Task SignOutAsync();

    /// <summary>
    /// 유저정보로 강제로그인
    /// </summary>
    /// <param name="user">The dbModelUser to sign-in.</param>
    /// <param name="isPersistent">Flag indicating whether the sign-in cookie should persist after the browser is closed.</param>
    /// <param name="authenticationMethod">Name of the method used to authenticate the dbModelUser.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    Task SignInAsync(TUser user, bool isPersistent, string? authenticationMethod = null);

    /// <summary>
    /// Returns true if the principal has an identity with the application cookie identity
    /// </summary>
    /// <returns>True if the dbModelUser is logged in with identity.</returns>
    Response IsSignedIn();


    /// <summary>
    /// Attempts to sign in the specified <paramref name="userName" /> and <paramref name="password" /> combination
    /// as an asynchronous operation.
    /// </summary>
    /// <param name="userName">The dbModelUser name to sign in.</param>
    /// <param name="password">The password to attempt to sign in with.</param>
    /// <param name="isPersistent">Flag indicating whether the sign-in cookie should persist after the browser is closed.</param>
    /// <param name="lockoutOnFailure">Flag indicating if the dbModelUser account should be locked if the sign in fails.</param>
    Task<Response> PasswordSignInAsync(string userName,string password,bool isPersistent,bool lockoutOnFailure);
}