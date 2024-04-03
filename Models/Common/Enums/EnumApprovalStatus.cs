namespace Models.Common.Enums;

/// <summary>
/// 승인상태 
/// </summary>
public enum EnumApprovalStatus
{
    /// <summary>
    /// 상태 없음
    /// </summary>
    None ,
    
    /// <summary>
    /// 세금계산서 발행 전
    /// </summary>
    PoNotYetPublished ,
    
    /// <summary>
    /// 세금계산서 발행
    /// </summary>
    PoPublished ,
    
    /// <summary>
    /// 인보이스 발행
    /// </summary>
    InVoicePublished
}