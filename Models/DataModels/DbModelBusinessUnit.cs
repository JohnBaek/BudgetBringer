using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Models.DataModels;

/// <summary>
/// 비즈니스 유닛 ( Bu )
/// </summary>
[Table("BusinessUnits")]
[Index(nameof(Name) , IsUnique = true)]
public class DbModelBusinessUnit
{
    /// <summary>
    /// 아이디 
    /// </summary>
    [Key]
    public Guid Id { get; init; }
    
    /// <summary>
    /// 유닛명 (유니크)
    /// </summary>
    [MaxLength(255)]
    [Required]
    public required string Name { get; init; }
    
    /// <summary>
    /// 등록일 (필수)
    /// </summary>
    [Required]
    public DateTime RegDate { get; init; }
    
    /// <summary>
    /// 수정일 (필수)
    /// </summary>
    [Required]
    public DateTime ModDate { get; init; }
}