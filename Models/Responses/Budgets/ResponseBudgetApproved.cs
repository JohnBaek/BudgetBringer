using System.ComponentModel.DataAnnotations;
using Models.Common.Enums;
using Models.Responses.Files;

namespace Models.Responses.Budgets;

/// <summary>
/// 예산정보 승인 정보 응답 클래스
/// </summary>
public class ResponseBudgetApproved : ResponseCommonWriter
{
    /// <summary>
    /// 예산 승인 모델 아이디 
    /// </summary>
    [Key]
    public Guid Id { get; init; }
    
    /// <summary>
    /// 승인일 ( 날짜가아닌 일반 스트링데이터도 포함 될 수 있다. )
    /// </summary>
    public required string ApprovalDate { get; init; }
    
    /// <summary>
    /// Base Year for Statistics ex ) 2024 .. 2025
    /// </summary>
    public int BaseYearForStatistics { get; set; }

    /// <summary>
    /// 500K 이상 예산 여부
    /// </summary>
    public bool IsAbove500K { get; init; }

    /// <summary>
    /// 설명 
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// 섹터 아이디
    /// </summary>
    public Guid SectorId { get; init; }

    /// <summary>
    /// DbModelBusinessUnit 아이디
    /// </summary>
    public Guid BusinessUnitId { get; init; }
    
    /// <summary>
    /// DbModelCostCenter 아이디
    /// </summary>
    public Guid CostCenterId { get; init; }
    
    /// <summary>
    /// DbModelCountryBusinessManager 아이디
    /// </summary>
    public Guid CountryBusinessManagerId { get; init; }
    
    /// <summary>
    /// DbModelSector 명
    /// </summary>
    public string SectorName { get; set; } = ""; 
    
    /// <summary>
    /// DbModelCostCenter 명
    /// </summary>
    public required string CostCenterName { get; init; } 

    /// <summary>
    /// DbModelCountryBusinessManager 명
    /// </summary>
    public required string CountryBusinessManagerName { get; init; } 
    
    /// <summary>
    /// DbModelBusinessUnit 명
    /// </summary>
    public required string BusinessUnitName { get; init; }

    /// <summary>
    /// 인보이스 번호 
    /// </summary>
    public int PoNumber { get; init; }

    // /// <summary>
    // /// 승인 상태 : PO 전/후 , Invoice 발행 여부  
    // /// </summary>
    // public EnumApprovalStatus ApprovalStatus { get; init; } 
    //
    /// <summary>
    /// 승인된 예산
    /// </summary>
    public double ApprovalAmount { get; init; }

    /// <summary>
    /// Not PO Issue Amount
    /// </summary>
    public double NotPoIssueAmount { get; set; }

    /// <summary>
    /// PO Issue Amount
    /// </summary>
    public double PoIssueAmount { get; set; }

    /// <summary>
    /// SpendingAndIssue PO Amount
    /// </summary>
    public double SpendingAndIssuePoAmount { get; set; }

    /// <summary>
    /// Actual
    /// </summary>
    public double Actual { get; init; }

    /// <summary>
    /// OcProjectName
    /// </summary>
    public string? OcProjectName { get; init; }
    
    /// <summary>
    /// BossLineDescription
    /// </summary>
    public string? BossLineDescription { get; init; }
    
    /// <summary>
    /// Attached files
    /// </summary>
    public List<ResponseFileUpload> AttachedFiles { get; set; } = new List<ResponseFileUpload>();
    
    /// <summary>
    /// File group ID
    /// </summary>
    public Guid? FileGroupId { get; set; } 
}