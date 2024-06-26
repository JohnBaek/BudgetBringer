using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.DataModels;

/// <summary>
/// 기본 값
/// </summary>
public class DbModelDefault
{
    /// <summary>
    /// 등록일 (필수)
    /// </summary>
    [Required]
    public DateTime RegDate { get; set; }
    
    /// <summary>
    /// 수정일 (필수)
    /// </summary>
    [Required]
    public DateTime ModDate { get; set; }

    /// <summary>
    /// 등록자 아이디 
    /// </summary>
    [Required]
    public Guid RegId { get; set; }
    
    /// <summary>
    /// 수정자 아이디
    /// </summary>
    [Required]
    public Guid ModId { get; set; }
    
    /// <summary>
    /// 등록자명 
    /// </summary>
    [Required]
    [MaxLength(255)]
    public required string RegName { get; set;}

    /// <summary>
    /// 수정자명 
    /// </summary>
    [Required]
    [MaxLength(255)]
    public required string ModName { get; set;}
    
    /// <summary>
    /// 등록자 아이디 왜래키 지정
    /// </summary>
    [ForeignKey("RegId")]
    public virtual DbModelUser? RegUser { get; set; }
    
    /// <summary>
    /// 수정자 아이디 왜래키 지정
    /// </summary>
    [ForeignKey("ModId")]
    public virtual DbModelUser? ModUser { get; set; }
}