using Models.Common.Enums;

namespace Models.Responses;

/// <summary>
/// 기초 응답 데이터 클래스 
/// </summary>
public class Response
{
    /// <summary>
    /// 기본 생성자 
    /// </summary>
    public Response()
    {
    }
    
    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="code">응답 코드</param>
    /// <param name="message">응답 메세지</param>
    /// <param name="result">응답 결과</param>
    public Response(EnumResponseResult result, string  code, string message )
    {
        Code = code;
        Message = message;
        Result = result;
    }
    

    /// <summary>
    /// 응답 코드 
    /// </summary>
    public string Code { get; set; } = "";
    
    /// <summary>
    /// 응답 메세지 
    /// </summary>
    public string Message { get; set; } = "";

    /// <summary>
    /// 권한 여부
    /// </summary>
    public bool IsAuthenticated { get; set; } = true;

    /// <summary>
    /// 응답 결과
    /// </summary>
    public EnumResponseResult Result { get; set; } = EnumResponseResult.Error;

    /// <summary>
    /// Is Response Success?
    /// </summary>
    public bool Success => Result == EnumResponseResult.Success;
    
    /// <summary>
    /// Is Response Error?
    /// </summary>
    public bool Error => Result == EnumResponseResult.Error;
    
    /// <summary>
    /// Is Response Warning?
    /// </summary>
    public bool Warning => Result == EnumResponseResult.Warning;
}