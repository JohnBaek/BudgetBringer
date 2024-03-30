using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Models.DataModels;

/// <summary>
/// 사용자 역할 정보
/// </summary>
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public class UserRole : IdentityUserRole<Guid>
{
    /// <summary>
    /// 사용자 정보 
    /// </summary>
    public User? User { get; init; }
    
    /// <summary>
    /// 역할 정보 
    /// </summary>
    public Role? Role { get; init; }
}