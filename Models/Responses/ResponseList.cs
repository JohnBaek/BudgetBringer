using System.Transactions;
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
    /// <param name="message">메세지</param>
    public ResponseList(string message)
    {
        this.Result = EnumResponseResult.Error;
        this.Message = message;
    }
    
    
    /// <summary>
    /// 응답 데이터 목록
    /// </summary>
    public List<T>? Items { get; set; } = new List<T>();
}