using Models.Common.Enums;

namespace Models.Responses;


/// <summary>
/// 응답 데이터 모델 T의 객체를 같이 포함하여 리턴한다.
/// </summary>
/// <typeparam name="T">T Data</typeparam>
public class ResponseData<T> : Response where T : class 
{
    /// <summary>
    /// 기본 생성자 
    /// </summary>
    public ResponseData()
    {
    }

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="message">메세지</param>
    public ResponseData(string message)
    {
        this.Result = EnumResponseResult.Error;
        this.Message = message;
    }
    
    /// <summary>
    /// 응답 데이터
    /// </summary>
    public T? Data { get; set; }
}