namespace Models.Common.Enums;

/// <summary>
/// 쿼리 SearchType 
/// </summary>
public enum EnumQuerySearchType
{
    /// <summary>
    /// 스트링 단일 검색
    /// </summary>
    Equals ,
    
    /// <summary>
    /// 스트링 Like 검색
    /// </summary>
    Contains ,
}