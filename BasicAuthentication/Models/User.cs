using Microsoft.AspNetCore.Identity;

namespace BasicAuthenticated.Resources;

/// <summary>
/// 사용자 데이터베이스 모델 클래스
/// </summary>
public class User : IdentityUser
{
    /// <summary>
    /// 로그인 아이디
    /// </summary>
    public string LoginId { get; set; }
}