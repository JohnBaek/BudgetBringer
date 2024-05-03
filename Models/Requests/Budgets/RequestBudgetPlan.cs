using System.ComponentModel.DataAnnotations;
using Models.Requests.Files;

namespace Models.Requests.Budgets;

/// <summary>
/// 예산정보 요청 클래스
/// </summary>
public class RequestBudgetPlan : RequestBase
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
    /// 통계에 포함시킬지 여부 ( false 일경우 통계에 잡히지 않음 )
    /// </summary>
    [Required(ErrorMessage = "통계 포함 여부를 입력해주세요")]
    public bool IsIncludeInStatistics { get; set; }

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
    /// DbModelBusinessUnit 아이디
    /// </summary>
    [Required(ErrorMessage = "비지니스 유닛 정보를 선택해주세요")]
    public Guid BusinessUnitId { get; init; }
    
    /// <summary>
    /// DbModelCostCenter 아이디
    /// </summary>
    [Required(ErrorMessage = "코스트센터 정보를 선택해주세요")]
    public Guid CostCenterId { get; init; }
    
    /// <summary>
    /// DbModelCountryBusinessManager 아이디
    /// </summary>
    [Required(ErrorMessage = "Country Business Manager 정보를 선택 해주세요")]
    public Guid CountryBusinessManagerId { get; init; }
    
    /// <summary>
    /// 총예산
    /// </summary>
    [Required(ErrorMessage = "총예산을 입력해주세요")]
    public double BudgetTotal { get; init; }

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

    /// <summary>
    /// 업로드된 파일을 영속화 시킬 리스트
    /// </summary>
    public List<RequestUploadFile> AttachedFiles { get; init; } = new List<RequestUploadFile>();
    
    /// <summary>
    /// FileGroupId
    /// </summary>
    public Guid? FileGroupId { get; set; }
}