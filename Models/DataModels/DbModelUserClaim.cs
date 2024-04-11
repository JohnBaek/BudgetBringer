using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace Models.DataModels;

/// <summary>
/// 사용자 Claim 
/// </summary>
public class DbModelUserClaim : IdentityUserClaim<Guid>
{
    /// <summary>
    /// 설명
    /// </summary>
    [MaxLength(3000)]
    public string? Description { get; init; }
    
    /// <summary>
    /// 사용자 정보 
    /// </summary>
    public virtual required DbModelUser DbModelUser { get; init; }
}