// using Models.DataModels;

using Models.DataModels;
using Models.Requests.Query;
using Models.Requests.Users;
using Models.Responses;
using Models.Responses.Users;

namespace Providers.Repositories.Interfaces;

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
    Task<DbModelUser?> GetUserWithIdPasswordAsync(string loginId, string password);

    /// <summary>
    /// 사용자의 Id 값으로 권한 값의 리스트를 가져온다.
    /// </summary>
    /// <param name="userId">사용자 아이디</param>
    /// <returns></returns>
    Task<List<string>> GetRolesByUserAsync(Guid userId);

    /// <summary>
    /// 로그인한 사용자의 정보를 가져온다.
    /// </summary>
    /// <returns></returns>
    Task<DbModelUser?> GetAuthenticatedUser();
    
    /// <summary>
    /// 리스트를 가져온다.
    /// </summary>
    /// <param name="requestQuery">쿼리 정보</param>
    /// <returns></returns>
    Task<ResponseList<ResponseUser>> GetListAsync(RequestQuery requestQuery);
    
    /// <summary>
    /// 패스워드를 변경한다 ( Role Admin 인 사용자만 가능 )
    /// </summary>
    /// <param name="request">패스워드 변경정보</param>
    /// <returns></returns>
    Task<Response> UpdatePasswordAsync(RequestUserChangePassword request);
}