using BasicAuthentication.ViewModels.Enums;

namespace BasicAuthentication.ViewModels.Responses;

public class Response
{
    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="result">로그인 결과</param>
    /// <param name="code">코드</param>
    /// <param name="message">메시지</param>
    public Response(EnumResponseResult result , string code, string message)
    {
        Result = result;
        Code = code;
        Message = message;
    }

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="code">코드</param>
    /// <param name="message">메시지</param>
    public Response(string code , string message)
    {
        Code = code;
        Message = message;
    }

    /// <summary>
    /// 응답 메세지 
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// 응답 코드 
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 응답 결과 
    /// </summary>
    public EnumResponseResult Result { get; set; } = EnumResponseResult.Error;
}