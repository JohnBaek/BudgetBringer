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
    [MaxLength(255)]
    public string Value { get; set; } = "";
}