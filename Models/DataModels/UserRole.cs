namespace Models.DataModels;

/// <summary>
/// 사용자 역할 정보 
/// </summary>
public partial class UserRole
{
    /// <summary>
    /// 사용자 아이디 
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// 역할 아이디
    /// </summary>
    public Guid RoleId { get; set; }
}