using System.ComponentModel;

namespace Models.Common.Enums;

/// <summary>
/// 승인상태 
/// </summary>
public enum EnumApprovalStatus
{
    /// <summary>
    /// 상태 없음
    /// </summary>
    [Description("상태 없음")]
    None ,
    
    /// <summary>
    /// 세금계산서 발행 전
    /// </summary>
    [Description("세금계산서 발행 전")]
    PoNotYetPublished ,
    
    /// <summary>
    /// 세금계산서 발행
    /// </summary>
    [Description("세금계산서 발행")]
    PoPublished ,
    
    /// <summary>
    /// 인보이스 발행
    /// </summary>
    [Description("인보이스 발행")]
    InVoicePublished
}