using Models.Common.Enums;

namespace Models.Requests.Budgets;

/// <summary>
/// Budget Plan Import Preview
/// </summary>
public class RequestBudgetPlanExcelImport : RequestBudgetPlan 
{
    /// <summary>
    /// Result of Request
    /// </summary>
    public EnumResponseResult Result { get; set; } = EnumResponseResult.Error;

    /// <summary>
    /// Message
    /// </summary>
    public string Message { get; set; } = "";

    /// <summary>
    /// To Insert?
    /// </summary>
    public bool ToInsert { get; set; } = false;
    
    /// <summary>
    /// DbModelSector 명
    /// </summary>
    public string SectorName { get; set; } = ""; 
    
    /// <summary>
    /// DbModelCostCenter 명
    /// </summary>
    public string CostCenterName { get; set; } 

    /// <summary>
    /// DbModelCountryBusinessManager 명
    /// </summary>
    public string CountryBusinessManagerName { get; set; } 
    
    /// <summary>
    /// DbModelBusinessUnit 명
    /// </summary>
    public string BusinessUnitName { get; set; }

}