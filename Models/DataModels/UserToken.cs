using System;
using System.Collections.Generic;

namespace Models.DataModels;

/// <summary>
/// 사용자의 로그인 토큰 정보
/// </summary>
public partial class UserToken
{
    /// <summary>
    /// 사용자의 고유 식별자
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// 로그인 제공자
    /// </summary>
    public string LoginProvider { get; set; } = null!;

    /// <summary>
    /// 토큰 이름
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 토큰 값
    /// </summary>
    public string? Value { get; set; }

    public virtual User User { get; set; } = null!;
}
