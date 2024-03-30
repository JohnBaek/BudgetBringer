using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Models.DataModels;

/// <summary>
/// 사용자 토큰
/// </summary>
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public class UserToken : IdentityUserToken<Guid>
{
    /// <summary>
    /// 유저
    /// </summary>
    public virtual User User { get; init; } = null!;
}
