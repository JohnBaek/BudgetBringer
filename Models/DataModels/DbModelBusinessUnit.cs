using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Models.DataModels;

/// <summary>
/// 비즈니스 유닛 ( Bu )
/// </summary>
[Table("BusinessUnits")]
[Index(nameof(Name) , IsUnique = true)]
public class DbModelBusinessUnit : DbModelDefault
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
    public required string Name { get; set; }
    
    /// <summary>
    /// 관계 정보
    /// </summary>
    public virtual ICollection<DbModelCountryBusinessManagerBusinessUnit>? CountryBusinessManagers { get; set; }
    
    /// <summary>
    /// Sequence 
    /// </summary>
    public int Sequence { get; set; } = 0;
}