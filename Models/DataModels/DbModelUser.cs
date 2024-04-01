using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
namespace Models.DataModels;

/// <summary>
/// Identity 사용자 정보 
/// </summary>
[Index(nameof(LoginId), IsUnique = true)]
public class DbModelUser : IdentityUser<Guid>
{
    /// <summary>
    /// 로그인 아이디 
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string LoginId { get; init; } = "";
    
    /// <summary>
    /// 컨트리 비지니스 매니저 아이디 
    /// </summary>
    [ForeignKey(nameof(DbModelCountryBusinessManager))]
    public Guid? CountryBusinessManagerId { get; init; }

    /// <summary>
    /// 사용자 명
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string DisplayName { get; init; } = "";

    /// <summary>
    /// 마지막 패스워드 변경일
    /// </summary>
    public DateTime? LastPasswordChangeDate { get; init; }
    
    /// <summary>
    /// 사용자 클레임 정보
    /// </summary>
    public virtual ICollection<DbModelUserClaim>? UserClaims { get; init; }
    
    /// <summary>
    /// 사용자 로그인 정보
    /// </summary>
    public virtual ICollection<DbModelUserLogin>? UserLogins { get; init; }
    
    /// <summary>
    /// 사용자 토큰정보
    /// </summary>
    public virtual ICollection<DbModelUserToken>? UserTokens { get; init; }
    
    /// <summary>
    /// 사용자 역할 정보 
    /// </summary>
    public virtual ICollection<DbModelUserRole>? UserRoles { get; init; }
    
    /// <summary>
    /// 사용자 액션 로그 정보
    /// </summary>
    public virtual ICollection<DbModelLogAction>? LogActions { get; init; } 
}