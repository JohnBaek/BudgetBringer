using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Common.Enums;

namespace Models.Responses.Budgets;

/// <summary>
/// 예산정보 승인 정보 응답 클래스
/// </summary>
public class ResponseBudgetApproved
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
    /// 승인일이 확인된경우 ( OC 승인 예정 등의 텍스트가 아니라 날짜 형태로 들어간 경우 ) 
    /// </summary>
    public required bool IsApproved { get; init; } 

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
    /// BusinessUnit 아이디
    /// </summary>
    public Guid BusinessUnitId { get; init; }
    
    /// <summary>
    /// CostCenter 아이디
    /// </summary>
    public Guid CostCenterId { get; init; }
    
    /// <summary>
    /// CountryBusinessManager 아이디
    /// </summary>
    public Guid CountryBusinessManagerId { get; init; }
    
    /// <summary>
    /// CostCenter 명
    /// </summary>
    public required string CostCenterName { get; init; } 

    /// <summary>
    /// CountryBusinessManager 명
    /// </summary>
    public required string CountryBusinessManagerName { get; init; } 
    
    /// <summary>
    /// BusinessUnit 명
    /// </summary>
    public required string BusinessUnitName { get; init; }

    /// <summary>
    /// 인보이스 번호 
    /// </summary>
    public int PoNumber { get; init; }

    /// <summary>
    /// 승인 상태 : PO 전/후 , Invoice 발행 여부  
    /// </summary>
    public EnumApprovalStatus ApprovalStatus { get; init; } 
    
    /// <summary>
    /// 승인된 예산
    /// </summary>
    public double ApprovalAmount { get; init; }
    
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
}