using Models.Common.Enums;

namespace Models.Responses;

/// <summary>
/// 응답 리스트 데이터 클래스
/// </summary>
public class ResponseList<T> : Response where T : class
{
    /// <summary>
    /// 기본 생성자
    /// </summary>
    public ResponseList()
    {
    }

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="result"></param>
    /// <param name="code"></param>
    /// <param name="message"></param>
    /// <param name="items"></param>
    public ResponseList(EnumResponseResult result, string code, string message, List<T>? items) 
        : base(result, code, message)
    {
        Items = items;
    }

    /// <summary>
    /// 스킵
    /// </summary>
    public int Skip { get; set; } = 0;

    /// <summary>
    /// 페이지 카운트 
    /// </summary>
    public int PageCount { get; set; } = 20;
    
    /// <summary>
    /// 전체 수
    /// </summary>
    public int TotalCount { get; set; } = 0;
    
    /// <summary>
    /// 응답 데이터 목록
    /// </summary>
    public List<T>? Items { get; set; } = new List<T>();
}