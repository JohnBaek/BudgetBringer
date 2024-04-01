using Microsoft.AspNetCore.Identity;

namespace Models.DataModels;

/// <summary>
/// 사용자 로그인
/// </summary>
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public class DbModelUserLogin : IdentityUserLogin<Guid>
{
    /// <summary>
    /// 사용자 정보 
    /// </summary>
    public virtual required DbModelUser DbModelUser { get; init; }
}