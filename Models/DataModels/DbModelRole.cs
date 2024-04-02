using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
namespace Models.DataModels;

/// <summary>
/// 역할 정보
/// </summary>
public class DbModelRole : IdentityRole<Guid>
{
    /// <summary>
    /// 사용자 역할 정보
    /// </summary>
    public virtual ICollection<DbModelUserRole>? UserRoles { get; init; }
    
    /// <summary>
    /// 역할 Claim 정보 
    /// </summary>
    public virtual ICollection<DbModelRoleClaim>? RoleClaims { get; init; }
}

