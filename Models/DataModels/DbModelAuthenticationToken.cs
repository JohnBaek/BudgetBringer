using Microsoft.AspNetCore.Identity;

namespace Models.DataModels;

/// <summary>
/// 사용자 토큰
/// </summary>
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public class DbModelAuthenticationToken
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpireDate { get; set; }

    /// <summary>
    /// User
    /// </summary>
    public virtual DbModelUser DbModelUser { get; init; } = null!;
}
