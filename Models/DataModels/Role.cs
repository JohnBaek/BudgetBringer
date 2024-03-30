using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
namespace Models.DataModels;

/// <summary>
/// 역할 정보
/// </summary>
public class Role : IdentityRole<Guid>
{
    /// <summary>
    /// 사용자 역할 정보
    /// </summary>
    public virtual ICollection<UserRole>? UserRoles { get; init; }
    
    /// <summary>
    /// 역할 Claim 정보 
    /// </summary>
    public virtual ICollection<RoleClaim>? RoleClaims { get; init; }
}