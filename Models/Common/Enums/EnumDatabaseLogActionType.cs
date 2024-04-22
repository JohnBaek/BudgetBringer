using System.ComponentModel;
using Features.Attributes;

namespace Models.Common.Enums;

/// <summary>
/// 데이터베이스 액션 정의
/// </summary>
public enum EnumDatabaseLogActionType
{
    /// <summary>
    /// 데이터추가
    /// </summary>
    [Description("추가")]
    [EnumColor("#7FFC77")]
    Add ,
    
    /// <summary>
    /// 데이터 업데이트 
    /// </summary>
    [EnumColor("#F1EF57")]    
    [Description("수정")]
    Update ,
    
    /// <summary>
    /// 삭제 
    /// </summary>
    [EnumColor("#FA6D47")]    
    [Description("삭제")]
    Delete ,
    
    /// <summary>
    /// 조회
    /// </summary>
    View ,
    
    /// <summary>
    /// 목록조회
    /// </summary>
    ViewList ,
}