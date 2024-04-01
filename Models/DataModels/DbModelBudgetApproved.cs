using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Common.Enums;

namespace Models.DataModels;

/// <summary>
/// 승인된 예산 데이터베이스 모델 클래스 
/// </summary>
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
[Table("DbModelBudgetApproved")]
public class DbModelBudgetApproved
{
    /// <summary>
    /// 예산 승인 모델 아이디 
    /// </summary>
    [Key]
    public Guid Id { get; init; }
    
    /// <summary>
    /// 승인일 ( 날짜가아닌 일반 스트링데이터도 포함 될 수 있다. )
    /// </summary>
    [MaxLength(255)]
    [Required]
    public required string ApprovalDate { get; init; } 

    /// <summary>
    /// 승인일이 확인된경우 ( OC 승인 예정 등의 텍스트가 아니라 날짜 형태로 들어간 경우 ) 
    /// </summary>
    [Required]
    public required bool IsApproved { get; init; } 

    /// <summary>
    /// 년도 정보 : yyyy
    /// </summary>
    [MaxLength(4)]
    public string? Year { get; init; } 

    /// <summary>
    /// 월 정보 : MM
    /// </summary>
    [MaxLength(2)]
    public string? Month { get; init; } 

    /// <summary>
    /// 일 정보 : dd
    /// </summary>
    [MaxLength(2)]
    public string? Day { get; init; } 

    /// <summary>
    /// 500K 이상 예산 여부
    /// </summary>
    public bool IsAbove500K { get; init; }

    /// <summary>
    /// 설명 
    /// </summary>
    [MaxLength(3000)]
    public string? Description { get; init; }

    /// <summary>
    /// 섹터 아이디
    /// </summary>
    [ForeignKey(nameof(DbModelSector))]
    public Guid SectorId { get; init; }

    /// <summary>
    /// DbModelBusinessUnit 아이디
    /// </summary>
    [ForeignKey(nameof(DbModelBusinessUnit))]
    public Guid BusinessUnitId { get; init; }
    
    
    /// <summary>
    /// DbModelCostCenter 아이디
    /// </summary>
    [ForeignKey(nameof(DbModelCostCenter))]
    public Guid CostCenterId { get; init; }

    
    /// <summary>
    /// DbModelCountryBusinessManager 아이디
    /// </summary>
    [ForeignKey(nameof(DbModelCountryBusinessManager))]
    public Guid CountryBusinessManagerId { get; init; }
    
    /// <summary>
    /// DbModelCostCenter 명
    /// </summary>
    [MaxLength(255)]
    [Required]
    public required string CostCenterName { get; init; } 

    /// <summary>
    /// DbModelCountryBusinessManager 명
    /// </summary>
    [MaxLength(255)]
    [Required]
    public required string CountryBusinessManagerName { get; init; } 
    
    /// <summary>
    /// DbModelBusinessUnit 명
    /// </summary>
    [MaxLength(255)]
    [Required]
    public required string BusinessUnitName { get; init; }

    /// <summary>
    /// 인보이스 번호 
    /// </summary>
    public int PoNumber { get; init; }

    /// <summary>
    /// 승인 상태 : PO 전/후 , Invoice 발행 여부  
    /// </summary>
    [Required]
    public EnumApprovalStatus ApprovalStatus { get; init; } 
    
    /// <summary>
    /// 승인된 예산
    /// </summary>
    [Required]
    [DefaultValue(0)]
    public double ApprovalAmount { get; init; }
    
    /// <summary>
    /// Actual
    /// </summary>
    [Required]
    [DefaultValue(0)]
    public double Actual { get; init; }

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
    public virtual required DbModelSector DbModelSector { get; init; }
    
    /// <summary>
    /// 비지니스 유닛
    /// </summary>
    public virtual required DbModelBusinessUnit DbModelBusinessUnit { get; init; }
    
    /// <summary>
    /// 코스트 센터
    /// </summary>
    public virtual required DbModelCostCenter DbModelCostCenter { get; init; }
    
    /// <summary>
    /// 컨트리 비지니스 매니저
    /// </summary>
    public virtual required DbModelCountryBusinessManager DbModelCountryBusinessManager { get; init; }
}