using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.DataModels;

/// <summary>
/// 예산 관련 데이터베이스 모델 클래스 
/// </summary>
[Table("BudgetPlans")]
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public partial class DbModelBudgetPlan : DbModelDefault
{
    /// <summary>
    /// 기본 생성자
    /// </summary>
    public DbModelBudgetPlan()
    {
    }


    /// <summary>
    /// 예산 모델 아이디 
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// 년도 정보 : yyyy
    /// </summary>
    [MaxLength(4)]
    public string Year { get; set; } = "";

    /// <summary>
    /// 월 정보 : MM
    /// </summary>
    [MaxLength(2)]
    public string Month { get; set; } = "";

    /// <summary>
    /// 일 정보 : dd
    /// </summary>
    [MaxLength(2)]
    public string Day { get; set; } = "";

    /// <summary>
    /// 500K 이상 예산 여부
    /// </summary>
    [Required]
    public bool IsAbove500K { get; set; }

    /// <summary>
    /// Is included in statistics
    /// </summary>
    public bool IsIncludeInStatistics { get; set; }
    

    /// <summary>
    /// 기안일 ( 날짜가아닌 일반 스트링데이터도 포함 될 수 있다. )
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string ApprovalDate { get; set; } = "";
    
    /// <summary>
    /// 기안일 정상 포맷 (yyyy-MM-dd) 이라면 DateOnly 로 파싱된 값 
    /// </summary>
    public DateOnly? ApproveDateValue { get; set; } 
    
    /// <summary>
    /// 기안일 정상 포맷 (yyyy-MM-dd) 여부
    /// </summary>
    [Required]
    public bool IsApprovalDateValid { get; set; }

    /// <summary>
    /// 설명 
    /// </summary>
    [MaxLength(3000)]
    public string? Description { get; set; }

    /// <summary>
    /// 섹터 아이디
    /// </summary>
    [ForeignKey(nameof(Sector))]
    public Guid SectorId { get; set; }

    /// <summary>
    /// DbModelBusinessUnit 아이디
    /// </summary>
    [ForeignKey(nameof(DbModelBusinessUnit))]
    public Guid BusinessUnitId { get; set; }
    
    
    /// <summary>
    /// DbModelCostCenter 아이디
    /// </summary>
    [ForeignKey(nameof(DbModelCostCenter))]
    public Guid CostCenterId { get; set; }

    
    /// <summary>
    /// DbModelCountryBusinessManager 아이디
    /// </summary>
    [ForeignKey(nameof(DbModelCountryBusinessManager))]
    public Guid CountryBusinessManagerId { get; set; }
    
    /// <summary>
    /// DbModelSector 명
    /// </summary>
    [MaxLength(255)]
    [Required]
    public string SectorName { get; set; } = ""; 
    
    /// <summary>
    /// DbModelCostCenter 명
    /// </summary>
    [MaxLength(255)]
    [Required]
    public string CostCenterName { get; set; } = ""; 

    /// <summary>
    /// DbModelCountryBusinessManager 명
    /// </summary>
    [MaxLength(255)]
    [Required]
    public string CountryBusinessManagerName { get; set; } = ""; 
    
    /// <summary>
    /// DbModelBusinessUnit 명
    /// </summary>
    [MaxLength(255)]
    [Required]
    public string BusinessUnitName { get; set; } = "";

    /// <summary>
    /// 총예산
    /// </summary>
    public double BudgetTotal { get; set; }

    /// <summary>
    /// OcProjectName
    /// </summary>
    [MaxLength(3000)]
    public string? OcProjectName { get; set; }
    
    /// <summary>
    /// BossLineDescription
    /// </summary>
    [MaxLength(3000)]
    public string? BossLineDescription { get; set; }
    
    /// <summary>
    /// 섹터
    /// </summary>
    public virtual DbModelSector? Sector { get; init; }
    
    /// <summary>
    /// 비지니스 유닛
    /// </summary> 
    public virtual DbModelBusinessUnit? DbModelBusinessUnit { get; init; }
    
    /// <summary>
    /// 코스트 센터
    /// </summary>
    public virtual DbModelCostCenter? DbModelCostCenter { get; init; }
    
    /// <summary>
    /// 컨트리 비지니스 매니저
    /// </summary>
    public virtual DbModelCountryBusinessManager? DbModelCountryBusinessManager { get; init; }
}