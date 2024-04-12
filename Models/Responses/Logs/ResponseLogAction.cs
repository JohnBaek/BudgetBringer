using Models.Common.Enums;

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
    public string RegName { get; init;} = "";

    /// <summary>
    /// 내용 
    /// </summary>
    public string Contents { get; init;} = "";

    /// <summary>
    /// 카테고리 
    /// </summary>
    public string Category { get; init; } = "";
    
    /// <summary>
    /// 액션타입
    /// </summary>
    public EnumDatabaseLogActionType ActionType { get; set; }

}