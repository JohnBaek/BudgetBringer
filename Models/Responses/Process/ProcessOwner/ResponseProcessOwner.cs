namespace Models.Responses.Process.ProcessOwner;

/// <summary>
/// 결과중 개별 ProcessOwner 별 통계 데이터 
/// </summary>
public class ResponseProcessOwner
{
    /// <summary>
    /// 컨트리 비지니스매니저 아이디 
    /// </summary>
    public Guid CountryBusinessManagerId { get; set; }

    /// <summary>
    /// 컨트리 비지니스매니저 명
    /// </summary>
    public string CountryBusinessManagerName { get; set; }  = "";

    /// <summary>
    /// 올년도 Budget ( ex: 2024FY )
    /// 당해년도 전체 예산
    /// </summary>
    public double BudgetYear { get; set; }
    
    /// <summary>
    /// 작년 Budget 확정된 것 ( ex: 2023FY )
    /// 승인된 전 년도 전체 예산
    /// </summary>
    public double BudgetApprovedYearBefore { get; set; }
    
    /// <summary>
    /// 올해 Budget 확정된 것 ( ex: 2024FY )
    /// 승인된 이번년도 전체 예산
    /// </summary>
    public double BudgetApprovedYear { get; set; }
    
    /// <summary>
    /// 올해 & 작년 Budget 확정된 것 ( ex: 2023FY&2024FY )
    /// 승인된 작년 + 이번년도 전체 예산
    /// </summary>
    public double BudgetApprovedYearSum { get; set; }
    
    /// <summary>
    /// 올해 남은 예산 ( BudgetYear - BudgetApprovedYearSum )
    /// 2024 년 남은 Budget
    /// [올해 Budget] - [승인된 작년 + 이번년도 전체 예산]
    /// </summary>
    public double BudgetRemainingYear { get; set; }
}