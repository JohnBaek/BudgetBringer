using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Models.DataModels;

/// <summary>
/// 코스트 센터 
/// </summary>
[Table("CostCenters")]
[Index(nameof(Value), IsUnique = true)]
public class DbModelCostCenter
{
    /// <summary>
    /// 아이디 
    /// </summary>
    [Key]
    public Guid Id { get; init; }
    
    /// <summary>
    /// DbModelCostCenter 값 (유니크)
    /// </summary>
    [Required]
    public int Value { get; init; }
    
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