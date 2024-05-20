using System.Text.Json.Serialization;

namespace Models.Responses.Process.ProcessBusinessUnit;

/// <summary>
/// 결과중 개별 BusinessUnit 별 통계 데이터 
/// </summary>
public class ResponseProcessBusinessUnit
{
    /// <summary>
    /// 비지니스 유닛 아이디 
    /// </summary>
    public Guid BusinessUnitId { get; set; } = Guid.NewGuid();

    /// <summary>
    /// 비지니스유닛 명
    /// </summary>
    public string BusinessUnitName { get; set; }  = "";

    /// <summary>
    /// 올년도 Budget ( ex: 2024FY ) 당해년도 전체 예산
    /// </summary>
    public double BudgetYear { get; set; }
    
    /// <summary>
    /// 올해 Budget 확정된 것 ( ex: 2024FY ) 승인된 이번년도 전체 예산
    /// </summary>
    public double ApprovedYear { get; set; }
        
    /// <summary>
    /// 올해 남은 예산 ( BudgetYear - BudgetApprovedYearSum ) 2024 년 남은 Budget [올해 Budget] - [승인된 작년 + 이번년도 전체 예산]
    /// </summary>
    public double RemainingYear { get; set; }
    
    /// <summary>
    /// RemainingYear divide ApprovedYear
    /// </summary>
    public double Ratio
    {
        get
        {
            if (RemainingYear == 0 || ApprovedYear == 0)
                return 0.0;
            
            return RemainingYear / ApprovedYear * 100;
        }
    }
    
    /// <summary>
    /// Sequence 
    /// </summary>
    [JsonIgnore]
    public int BusinessUnitSequence { get; set; } = 0;
    
    // /// <summary>
    // /// 작년 Budget 확정된 것 ( ex: 2023FY ) 승인된 전 년도 전체 예산
    // /// </summary>
    // public double BudgetApprovedYearBefore { get; set; }
    //
    // /// <summary>
    // /// 올해 Budget 확정된 것 ( ex: 2024FY ) 승인된 이번년도 전체 예산
    // /// </summary>
    // public double BudgetApprovedYear { get; set; }
    //
    // /// <summary>
    // /// 올해 작년 Budget 확정된 것 ( ex: 2023FY 2024FY ) 승인된 작년 + 이번년도 전체 예산
    // /// </summary>
    // public double BudgetApprovedYearSum { get; set; }
    //
    // /// <summary>
    // /// 올해 남은 예산 ( BudgetYear - BudgetApprovedYearSum ) 2024 년 남은 Budget [올해 Budget] - [승인된 작년 + 이번년도 전체 예산]
    // /// </summary>
    // public double BudgetRemainingYear { get; set; }
}