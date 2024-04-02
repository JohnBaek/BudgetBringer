using Models.Common.Enums;
using Models.Requests.Query;

namespace Models.Responses;

/// <summary>
/// 응답 리스트 데이터 클래스
/// </summary>
public class ResponseList<T> : Response where T : class
{
    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="result">응답결과</param>
    /// <param name="requestQuery">쿼리정보</param>
    public ResponseList(EnumResponseResult result , RequestQuery requestQuery)
    {
        this.Result = result;
        this.Skip = requestQuery.Skip;
        this.PageCount = requestQuery.PageCount;
    }

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="result">응답결과</param>
    /// <param name="requestQuery">쿼리정보</param>
    /// <param name="items">데이터</param>
    public ResponseList(EnumResponseResult result , RequestQuery requestQuery , List<T> items)
    {
        Result = result;
        Skip = requestQuery.Skip;
        PageCount = requestQuery.PageCount;
        Items = items;
    }
    
    
    /// <summary>
    /// 기본 생성자
    /// </summary>
    public ResponseList()
    {
    }   
    
    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="message">메세지</param>
    public ResponseList(string message)
    {
        Result = EnumResponseResult.Error;
        Message = message;
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