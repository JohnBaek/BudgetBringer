using System.ComponentModel.DataAnnotations;
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
    /// 역할 정보
    /// </summary>
    public virtual Role Role { get; init; } = null!;

    /// <summary>
    /// 타입정보 
    /// </summary>
    [Required]
    [MaxLength(255)]
    public sealed override string? ClaimType { get; set; }
    
    /// <summary>
    /// 타입 값
    /// </summary>
    [Required]
    [MaxLength(255)]
    public sealed override string? ClaimValue { get; set; }

    /// <summary>
    /// Claim 명 
    /// </summary>
    [MaxLength(255)]
    public string? DisplayName { get; init; }
    
    /// <summary>
    /// Claim 설명
    /// </summary>
    [MaxLength(1000)]
    public string? Description { get; init; }
}