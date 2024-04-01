namespace Models.Responses.Users;

/// <summary>
/// 사용자 역할 클래스 
/// </summary>
public class ResponseUserRole
{
    /// <summary>
    /// 역할명 
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// 역할 상세 Claim
    /// </summary>
    public List<ResponseUserRoleClaim>? Claims { get; set; }
}