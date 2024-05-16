using System.ComponentModel.DataAnnotations;
using Models.Common.Enums;
using Models.Requests.Files;

namespace Models.Requests.Budgets;

/// <summary>
/// 예산정보 승인 정보 요청 클래스
/// </summary>
public class RequestBudgetApproved : RequestBase
{
    /// <summary>
    /// 500K 이상 예산 여부
    /// </summary>
    [Required(ErrorMessage = "500K 이상 예산 여부를 입력해주세요")]
    public bool IsAbove500K { get; set; }

    /// <summary>
    /// 기안일 ( 날짜가아닌 일반 스트링데이터도 포함 될 수 있다. )
    /// </summary>
    [Required(ErrorMessage = "기안일을 입력해주세요")]
    public string ApprovalDate { get; set; } = "";
    
    /// <summary>
    /// Base Year for Statistics ex ) 2024 .. 2025
    /// </summary>
    [Range(typeof(int), "2013", "2050", ErrorMessage = "유효한 년도가 아닙니다.")]
    public int BaseYearForStatistics { get; set; }

    /// <summary>
    /// 설명 
    /// </summary>
    [MaxLength(3000, ErrorMessage = "설명은 3000자 이하로 입력해주세요")]
    public string? Description { get; set; }

    /// <summary>
    /// 섹터 아이디
    /// </summary>
    [Required(ErrorMessage = "섹터 정보를 선택해주세요")]
    public Guid SectorId { get; set; }

    /// <summary>
    /// DbModelBusinessUnit 아이디
    /// </summary>
    [Required(ErrorMessage = "비지니스 유닛 정보를 선택해주세요")]
    public Guid BusinessUnitId { get; set; }
    
    /// <summary>
    /// DbModelCostCenter 아이디
    /// </summary>
    [Required(ErrorMessage = "코스트센터 정보를 선택해주세요")]
    public Guid CostCenterId { get; set; }
    
    /// <summary>
    /// DbModelCountryBusinessManager 아이디
    /// </summary>
    [Required(ErrorMessage = "Country Business Manager 정보를 선택 해주세요")]
    public Guid CountryBusinessManagerId { get; set; }
    
    /// <summary>
    /// 인보이스 번호 
    /// </summary>
    public int PoNumber { get; set; }

    /// <summary>
    /// 승인 상태 : PO 전/후 , Invoice 발행 여부  
    /// </summary>
    [Required(ErrorMessage = "승인 상태 를 선택해주세요")]
    public EnumApprovalStatus ApprovalStatus { get; set; } 
    
    /// <summary>
    /// 승인된 예산
    /// </summary>
    public double ApprovalAmount { get; set; }
    
    /// <summary>
    /// Actual
    /// </summary>
    public double Actual { get; set; }

    /// <summary>
    /// OcProjectName
    /// </summary>
    [MaxLength(3000, ErrorMessage = "OcProjectName은 3000자 이하로 입력해주세요")]
    public string? OcProjectName { get; set; }
    
    /// <summary>
    /// BossLineDescription
    /// </summary>
    [MaxLength(3000, ErrorMessage = "BossLineDescription은 3000자 이하로 입력해주세요")]
    public string? BossLineDescription { get; set; }
    
    /// <summary>
    /// 업로드된 파일을 영속화 시킬 리스트
    /// </summary>
    public List<RequestUploadFile> AttachedFiles { get; set; } = new List<RequestUploadFile>();
    
    /// <summary>
    /// FileGroupId
    /// </summary>
    public Guid? FileGroupId { get; set; }
}