using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Common.Enums;

namespace Models.DataModels;

/// <summary>
/// 승인된 예산 데이터베이스 모델 클래스 
/// </summary>
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
[Table("BudgetApproved")]
public class DbModelBudgetApproved : DbModelDefault
{
    /// <summary>
    /// 예산 승인 모델 아이디 
    /// </summary>
    [Key]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Id of Group It could be null 
    /// </summary>
    public Guid? FileGroupId { get; set; }

    /// <summary>
    /// 승인일 ( 날짜가아닌 일반 스트링데이터도 포함 될 수 있다. )
    /// </summary>
    [MaxLength(255)]
    [MinLength(4)]
    [Required]
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
    /// 승인일이 확인된경우 ( OC 승인 예정 등의 텍스트가 아니라 날짜 형태로 들어간 경우 ) 
    /// </summary>
    [Required]
    public bool IsApproved { get; set; } 

    /// <summary>
    /// 년도 정보 : yyyy
    /// </summary>
    [MaxLength(4)]
    public string? Year { get; set; } 

    /// <summary>
    /// 월 정보 : MM
    /// </summary>
    [MaxLength(2)]
    public string? Month { get; set; } 

    /// <summary>
    /// 일 정보 : dd
    /// </summary>
    [MaxLength(2)]
    public string? Day { get; set; } 

    /// <summary>
    /// 500K 이상 예산 여부
    /// </summary>
    public bool IsAbove500K { get; set; }

    /// <summary>
    /// 설명 
    /// </summary>
    [MaxLength(3000)]
    public string? Description { get; set; }

    /// <summary>
    /// 섹터 아이디
    /// </summary>
    [ForeignKey(nameof(DbModelSector))]
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
    public string CostCenterName { get; set; }  = "";

    /// <summary>
    /// DbModelCountryBusinessManager 명
    /// </summary>
    [MaxLength(255)]
    [Required]
    public string CountryBusinessManagerName { get; set; }  = "";
    
    /// <summary>
    /// DbModelBusinessUnit 명
    /// </summary>
    [MaxLength(255)]
    [Required]
    public string BusinessUnitName { get; set; } = "";

    /// <summary>
    /// 인보이스 번호 
    /// </summary>
    public int PoNumber { get; set; }

    /// <summary>
    /// 승인 상태 : PO 전/후 , Invoice 발행 여부  
    /// </summary>
    [Required]
    public EnumApprovalStatus ApprovalStatus { get; set; } 
    
    /// <summary>
    /// 승인된 예산
    /// </summary>
    [Required]
    [DefaultValue(0)]
    public double ApprovalAmount { get; set; }
    
    /// <summary>
    /// Actual
    /// </summary>
    [Required]
    [DefaultValue(0)]
    public double Actual { get; set; }

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
    public virtual DbModelSector? DbModelSector { get; init; }
    
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