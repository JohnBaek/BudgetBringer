namespace Models.Responses.Logs;

/// <summary>
/// 액션 로그 응답 클래스 
/// </summary>
public class ResponseLogAction
{
    /// <summary>
    /// 등록일
    /// </summary>
    public DateTime RegDate { get; init;}

    /// <summary>
    /// 등록자명 
    /// </summary>
    public required string RegName { get; init;}

    /// <summary>
    /// 내용 
    /// </summary>
    public required string Contents { get; init;}

}