using System;
using System.Collections.Generic;

namespace Models.DataModels;

/// <summary>
/// 사용자의 로그인 관련 정보
/// </summary>
public partial class UserLogin
{
    /// <summary>
    /// 로그인 제공자
    /// </summary>
    public string LoginProvider { get; set; } = null!;

    /// <summary>
    /// 제공자 키
    /// </summary>
    public string ProviderKey { get; set; } = null!;

    /// <summary>
    /// 제공자의 표시 이름
    /// </summary>
    public string? ProviderDisplayName { get; set; }

    /// <summary>
    /// 사용자의 고유 식별자
    /// </summary>
    public Guid UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
