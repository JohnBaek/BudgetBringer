namespace Models.Responses.Users;

/// <summary>
/// 사용자 역할에 대한 Claim
/// </summary>
public class ResponseUserRoleClaim
{
    /// <summary>
    /// Type 명
    /// </summary>
    public required string Type { get; set; }

    /// <summary>
    /// 값
    /// </summary>
    public required string Value { get; set; }
}