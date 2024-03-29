using Models.Common.Enums;

namespace Models.Responses;

/// <summary>
/// 응답 리스트 데이터 클래스
/// </summary>
public class ResponseList<T> : Response where T : class
{
    /// <summary>
    /// 응답 데이터 목록
    /// </summary>
    public List<T>? Items { get; set; } = new List<T>();
}