using Microsoft.AspNetCore.Identity;

namespace Models.DataModels;

/// <summary>
/// 사용자 토큰
/// </summary>
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public class DbModelUserToken : IdentityUserToken<Guid>
{
    /// <summary>
    /// 유저
    /// </summary>
    public virtual DbModelUser DbModelUser { get; init; } = null!;
}
