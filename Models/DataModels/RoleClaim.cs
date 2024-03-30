using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Models.DataModels;

/// <summary>
/// 역할 Claim
/// </summary>
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public class RoleClaim : IdentityRoleClaim<Guid>
{
    /// <summary>
    /// 사용자 정보 
    /// </summary>
    public virtual required Role Role { get; init; }
}