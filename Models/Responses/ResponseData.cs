using Models.Common.Enums;
using Models.DataModels;

namespace Models.Responses;


/// <summary>
/// 응답 데이터 모델 T의 객체를 같이 포함하여 리턴한다.
/// </summary>
/// <typeparam name="T">T Data</typeparam>
public class ResponseData<T> : Response
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
    /// <param name="result"></param>
    /// <param name="code"></param>
    /// <param name="message"></param>
    /// <param name="data"></param>
    public ResponseData(EnumResponseResult result, string code, string message, T? data) : base(result, code, message)
    {
        Result = result;
        Code = code;
        Message = message;
        Data = data;
    }


    /// <summary>
    /// 응답 데이터
    /// </summary>
    public T? Data { get; set; }
}