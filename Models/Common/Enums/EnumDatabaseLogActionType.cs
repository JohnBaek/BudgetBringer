namespace Models.Common.Enums;

/// <summary>
/// 데이터베이스 액션 정의
/// </summary>
public enum EnumDatabaseLogActionType
{
    /// <summary>
    /// 데이터추가
    /// </summary>
    Add ,
    
    /// <summary>
    /// 데이터 업데이트 
    /// </summary>
    Update ,
    
    /// <summary>
    /// 삭제 
    /// </summary>
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