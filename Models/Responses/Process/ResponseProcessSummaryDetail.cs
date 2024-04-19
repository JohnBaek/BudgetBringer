namespace Models.Responses.Process;

/// <summary>
/// Represent Summary Details  
/// </summary>
/// <typeparam name="T"></typeparam>
public class ResponseProcessSummaryDetail<T> where T : class 
{
    /// <summary>
    /// Sequence 
    /// </summary>
    public int Sequence { get; set; } = 0;

    /// <summary>
    /// Title
    /// </summary>
    public string Title { get; set; } = "";

    /// <summary>
    /// 상세 정보 리스트
    /// </summary>
    public List<T> Items { get; set; } = new List<T>();
}