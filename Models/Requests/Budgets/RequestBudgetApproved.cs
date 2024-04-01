using System.ComponentModel.DataAnnotations;
using Models.Common.Enums;

namespace Models.Requests.Budgets;

/// <summary>
/// 예산정보 승인 정보 응답 클래스
/// </summary>
public class RequestBudgetApproved
{
    /// <summary>
    /// 500K 이상 예산 여부
    /// </summary>
    [Required(ErrorMessage = "500K 이상 예산 여부를 입력해주세요")]
    public bool IsAbove500K { get; init; }

    /// <summary>
    /// 기안일 ( 날짜가아닌 일반 스트링데이터도 포함 될 수 있다. )
    /// </summary>
    [Required(ErrorMessage = "기안일을 입력해주세요")]
    public string ApprovalDate { get; init; } = "";


    /// <summary>
    /// 설명 
    /// </summary>
    [MaxLength(3000, ErrorMessage = "설명은 3000자 이하로 입력해주세요")]
    public string? Description { get; init; }

    /// <summary>
    /// 섹터 아이디
    /// </summary>
    [Required(ErrorMessage = "섹터 정보를 선택해주세요")]
    public Guid SectorId { get; init; }

    /// <summary>
    /// BusinessUnit 아이디
    /// </summary>
    [Required(ErrorMessage = "비지니스 유닛 정보를 선택해주세요")]
    public Guid BusinessUnitId { get; init; }
    
    /// <summary>
    /// CostCenter 아이디
    /// </summary>
    [Required(ErrorMessage = "코스트센터 정보를 선택해주세요")]
    public Guid CostCenterId { get; init; }
    
    /// <summary>
    /// CountryBusinessManager 아이디
    /// </summary>
    [Required(ErrorMessage = "Country Business Manager 정보를 선택 해주세요")]
    public Guid CountryBusinessManagerId { get; init; }
    
    /// <summary>
    /// 인보이스 번호 
    /// </summary>
    public int PoNumber { get; init; }

    /// <summary>
    /// 승인 상태 : PO 전/후 , Invoice 발행 여부  
    /// </summary>
    [Required(ErrorMessage = "승인 상태 를 선택해주세요")]
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
    [MaxLength(3000, ErrorMessage = "OcProjectName은 3000자 이하로 입력해주세요")]
    public string? OcProjectName { get; init; }
    
    /// <summary>
    /// BossLineDescription
    /// </summary>
    [MaxLength(3000, ErrorMessage = "BossLineDescription은 3000자 이하로 입력해주세요")]
    public string? BossLineDescription { get; init; }
}