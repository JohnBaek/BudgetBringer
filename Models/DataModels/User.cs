using Microsoft.AspNetCore.Identity;

namespace Models.DataModels;

/// <summary>
/// Identity 사용자 정보 
/// </summary>
public partial class User : IdentityUser<Guid>
{
    /// <summary>
    /// 로그인 아이디 
    /// </summary>
    public string LoginId { get; set; }

    /// <summary>
    /// 사용자 명
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    /// 마지막 패스워드 변경일
    /// </summary>
    public DateTime LastPasswordChangeDate { get; set; }
}