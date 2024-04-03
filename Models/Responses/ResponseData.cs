using Models.Common.Enums;
using Models.DataModels;

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
    /// <param name="code"></param>
    /// <param name="message"></param>
    public ResponseData( string code, string message) 
    {
        Code = code;
        Message = message;
    }

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="result"></param>
    /// <param name="data"></param>
    public ResponseData(EnumResponseResult result, T? data) : base(result)
    {
        Data = data;
    }

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="result"></param>
    /// <param name="code"></param>
    /// <param name="message"></param>
    public ResponseData(EnumResponseResult result, string code, string message) : base(result, code, message)
    {
        Result = result;
        Code = code;
        Message = message;
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