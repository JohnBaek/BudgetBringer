namespace Models.Responses.Budgets;

/// <summary>
/// 예산정보 응답 클래스
/// </summary>
public class ResponseBudgetPlan : ResponseCommonWriter
{
    /// <summary>
    /// 예산 모델 아이디 
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// 500K 이상 예산 여부
    /// </summary>
    public bool IsAbove500K { get; init; }

    /// <summary>
    /// 기안일 ( 날짜가아닌 일반 스트링데이터도 포함 될 수 있다. )
    /// </summary>
    public string ApprovalDate { get; init; } = "";
    
    /// <summary>
    /// Is included in statistics
    /// </summary>
    public bool IsIncludeInStatistics { get; set; }
    
    /// <summary>
    /// 기안일 정상 포맷 (yyyy-MM-dd) 이라면 DateOnly 로 파싱된 값 
    /// </summary>
    public DateOnly? ApproveDateValue { get; set; } 
    
    /// <summary>
    /// 기안일 정상 포맷 (yyyy-MM-dd) 여부
    /// </summary>
    public bool IsApprovalDateValid { get; set; }

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
    /// 총예산
    /// </summary>
    public double BudgetTotal { get; init; }

    /// <summary>
    /// OcProjectName
    /// </summary>
    public string? OcProjectName { get; init; }
    
    /// <summary>
    /// BossLineDescription
    /// </summary>
    public string? BossLineDescription { get; init; }
}