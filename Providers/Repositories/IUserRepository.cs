// using Models.DataModels;

using Microsoft.AspNetCore.Identity;
using Models.DataModels;
using Models.Responses.Users;

namespace Providers.Repositories;

/// <summary>
/// 유저 정보 리파지토리 인터페이스
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// 사용자의 아이디와 패스워드로 사용자가 존재하는지 확인한다.
    /// </summary>
    /// <param name="loginId">로그인 아이디</param>
    /// <returns>결과</returns>
    Task<bool>  ExistUserAsync(string loginId);

    /// <summary>
    /// 사용자의 아이디와 패스워드로 사용자를 가져온다.
    /// </summary>
    /// <param name="loginId">로그인 아이디</param>
    /// <param name="password">패스워드 (SHA 256 인크립트 된 원본)</param>
    /// <returns>결과</returns>
    Task<User> GetUserWithIdPasswordAsync(string loginId, string password);
}