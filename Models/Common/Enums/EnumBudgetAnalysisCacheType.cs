namespace Models.Common.Enums;

/// <summary>
/// 통계 결과 캐시 종류 
/// </summary>
public enum EnumBudgetAnalysisCacheType
{
    /// <summary>
    /// 오너별
    /// </summary>
    ProcessOwner ,
    
    /// <summary>
    /// 비지니스 유닛 별 
    /// </summary>
    ProcessBusinessUnit ,
    
    /// <summary>
    /// 승인된 건 별
    /// </summary>
    ProcessApproved ,
}