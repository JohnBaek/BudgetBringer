using System;
using System.Collections.Generic;

namespace Models.DataModels;

/// <summary>
/// 역할 별 권한 정보
/// </summary>
public partial class RoleClaim
{
    /// <summary>
    /// 역할 권한의 고유 식별자
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 역할의 고유 식별자
    /// </summary>
    public Guid RoleId { get; set; }

    /// <summary>
    /// 권한 유형
    /// </summary>
    public string? ClaimType { get; set; }

    /// <summary>
    /// 권한 값
    /// </summary>
    public string? ClaimValue { get; set; }

    public virtual Role Role { get; set; } = null!;
}
