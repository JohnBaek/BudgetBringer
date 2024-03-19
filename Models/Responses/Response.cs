using Models.Common.Enums;

namespace Models.Responses;

/// <summary>
/// 기초 응답 데이터 클래스 
/// </summary>
public class Response
{
    /// <summary>
    /// 응답 코드 
    /// </summary>
    public string Code { get; set; } = "";
    
    /// <summary>
    /// 응답 메세지 
    /// </summary>
    public string Message { get; set; } = "";

    /// <summary>
    /// 응답 결과
    /// </summary>
    public EnumResponseResult Result { get; set; } = EnumResponseResult.Error;
}