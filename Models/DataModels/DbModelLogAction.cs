using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Common.Enums;

// ReSharper disable All
namespace Models.DataModels;

/// <summary>
/// 액션 로그 
/// </summary>
[Table("DbModelLogAction")]
public class DbModelLogAction 
{
    /// <summary>
    /// 아이디 값 
    /// </summary>
    [Key]
    public Guid Id { get; init;}

    /// <summary>
    /// 등록일
    /// </summary>
    [Required]
    public DateTime RegDate { get; init;}

    /// <summary>
    /// 등록자 아이디 
    /// </summary>
    [ForeignKey("DbModelUser")]
    public Guid? RegId { get; init;}

    /// <summary>
    /// 등록자명 
    /// </summary>
    [MaxLength(255)]
    public string? RegName { get; init;}

    /// <summary>
    /// 내용 
    /// </summary>
    [Required]
    [MaxLength(3000)]
    public required string Contents { get; init;}
    
    /// <summary>
    /// 액션타입
    /// </summary>
    [Required]
    public EnumDatabaseLogActionType ActionType { get; set; }
    
    /// <summary>
    /// 사용자 정보
    /// </summary>
    public virtual DbModelUser? DbModelUser { get; init;} 
    
}