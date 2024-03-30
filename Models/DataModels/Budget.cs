using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.DataModels;

/// <summary>
/// 예산 관련 데이터베이스 모델 클래스 
/// </summary>
[Table("Budgets")]
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public partial class Budget
{
    /// <summary>
    /// 예산 모델 아이디 
    /// </summary>
    [Key]
    public Guid Id { get; init; }

    /// <summary>
    /// 년도 정보 : yyyy
    /// </summary>
    [MaxLength(4)]
    public string Year { get; init; } = "";

    /// <summary>
    /// 월 정보 : MM
    /// </summary>
    [MaxLength(2)]
    public string Month { get; init; } = "";

    /// <summary>
    /// 일 정보 : dd
    /// </summary>
    [MaxLength(2)]
    public string Day { get; init; } = "";

    /// <summary>
    /// 500K 이상 예산 여부
    /// </summary>
    [Required]
    public bool IsAbove500K { get; init; }

    /// <summary>
    /// 기안일 ( 날짜가아닌 일반 스트링데이터도 포함 될 수 있다. )
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string ApprovalDate { get; init; } = "";

    /// <summary>
    /// 설명 
    /// </summary>
    [MaxLength(3000)]
    public string? Description { get; init; }

    /// <summary>
    /// 섹터 아이디
    /// </summary>
    [ForeignKey(nameof(Sector))]
    public Guid SectorId { get; init; }

    /// <summary>
    /// BusinessUnit 아이디
    /// </summary>
    [ForeignKey(nameof(BusinessUnit))]
    public Guid BusinessUnitId { get; init; }
    
    
    /// <summary>
    /// CostCenter 아이디
    /// </summary>
    [ForeignKey(nameof(CostCenter))]
    public Guid CostCenterId { get; init; }

    
    /// <summary>
    /// CountryBusinessManager 아이디
    /// </summary>
    [ForeignKey(nameof(CountryBusinessManager))]
    public Guid CountryBusinessManagerId { get; init; }
    
    /// <summary>
    /// CostCenter 명
    /// </summary>
    [MaxLength(255)]
    [Required]
    public required string CostCenterName { get; init; } 

    /// <summary>
    /// CountryBusinessManager 명
    /// </summary>
    [MaxLength(255)]
    [Required]
    public required string CountryBusinessManagerName { get; init; } 
    
    /// <summary>
    /// BusinessUnit 명
    /// </summary>
    [MaxLength(255)]
    [Required]
    public required string BusinessUnitName { get; init; }

    /// <summary>
    /// 총예산
    /// </summary>
    public double BudgetTotal { get; init; }

    /// <summary>
    /// OcProjectName
    /// </summary>
    [MaxLength(3000)]
    public string? OcProjectName { get; init; }
    
    /// <summary>
    /// BossLineDescription
    /// </summary>
    [MaxLength(3000)]
    public string? BossLineDescription { get; init; }
    
    /// <summary>
    /// 섹터
    /// </summary>
    public virtual required Sector? Sector { get; init; }
    
    /// <summary>
    /// 비지니스 유닛
    /// </summary> 
    public virtual required BusinessUnit BusinessUnit { get; init; }
    
    /// <summary>
    /// 코스트 센터
    /// </summary>
    public virtual required CostCenter CostCenter { get; init; }
    
    /// <summary>
    /// 컨트리 비지니스 매니저
    /// </summary>
    public virtual required CountryBusinessManager CountryBusinessManager { get; init; }
}