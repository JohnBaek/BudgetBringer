namespace Models.Responses.Users;

/// <summary>
/// 사용자 정보 응답 클래스 
/// </summary>
public class ResponseUser 
{
    /// <summary>
    /// 아이디 
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// 이름 
    /// </summary>
    public string Name { get; set; } = "";
    
    /// <summary>
    /// 로그인 아이디 
    /// </summary>
    public string LoginId { get; init; } = "";

    /// <summary>
    /// 사용자 권한
    /// </summary>
    public List<ResponseUserRole> Roles { get; set; } = new List<ResponseUserRole>();
}