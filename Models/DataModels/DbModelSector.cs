using System.ComponentModel.DataAnnotations;

namespace Models.DataModels;

/// <summary>
/// 섹터 정보 
/// </summary>
public class DbModelSector : DbModelDefault
{
    /// <summary>
    /// 섹터 아이디 
    /// </summary>
    [Key]
    public Guid Id { get; init; }

    /// <summary>
    /// 섹터 값
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