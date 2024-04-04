namespace Models.Common.Query;

/// <summary>
/// 검색 정보 정의
/// </summary>
public class QuerySearch
{
    /// <summary>
    /// 필드정보 
    /// </summary>
    public required string Field { get; set; }


    /// <summary>
    /// 키워드정보
    /// </summary>
    private readonly string? _keyword;
    public string? Keyword
    {
        get => _keyword!.Replace("\\b","").Replace("\b","");
        init => _keyword = value;
    }
}