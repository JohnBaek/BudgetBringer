namespace Models.Requests.Query;

/// <summary>
/// Query 요청 
/// </summary>
public class RequestQuery
{
    /// <summary>
    /// 스킵
    /// </summary>
    public int Skip { get; set; } = 0;

    /// <summary>
    /// 페이지 카운트 
    /// </summary>
    public int PageCount { get; set; } = 20;

    /// <summary>
    /// 검색 키워드 
    /// </summary>
    public List<string> SearchKeywords { get; set; } = new List<string>();
    
    /// <summary>
    /// 검색 필드
    /// </summary>
    public List<string> SearchFields { get; set; } = new List<string>();
}