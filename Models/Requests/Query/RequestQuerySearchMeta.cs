using Models.Common.Enums;

namespace Models.Requests.Query;

/// <summary>
/// 쿼리 SearchMeta 
/// </summary>
public class RequestQuerySearchMeta
{
    /// <summary>
    /// 쿼리 검색 타입
    /// </summary>
    public EnumQuerySearchType SearchType { get; set; } = EnumQuerySearchType.String;

    /// <summary>
    /// 필드명
    /// </summary>
    public string Field { get; set; } = "";
}