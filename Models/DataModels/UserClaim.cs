using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace Models.DataModels;

/// <summary>
/// 사용자 Claim 
/// </summary>
public class UserClaim : IdentityUserClaim<Guid>
{
    /// <summary>
    /// 사용자 정보 
    /// </summary>
    public virtual required User User { get; init; }
}