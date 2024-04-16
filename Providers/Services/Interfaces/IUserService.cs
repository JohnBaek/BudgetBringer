using Models.Requests.Query;
using Models.Requests.Users;
using Models.Responses;
using Models.Responses.Users;

namespace Providers.Services.Interfaces;

/// <summary>
/// 유저 서비스 인터페이스
/// </summary>
public interface IUserService
{
    /// <summary>
    /// 로그인한 사용자의 권한 목록을 가져온다.
    /// </summary>
    /// <returns></returns>
    Task<ResponseList<ResponseUserRole>> GetRolesByUserAsync();

    /// <summary>
    /// 로그인한 사용자의 정보를 가져온다.
    /// </summary>
    /// <returns></returns>
    Task<ResponseData<ResponseUser>> GetUserAsync();
    
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