using System;
using System.Collections.Generic;

namespace Models.DataModels;

/// <summary>
/// 사용자 정보
/// </summary>
public partial class User
{
    /// <summary>
    /// 아이디 값
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 유저 타입 구분자
    /// </summary>
    public string Discriminator { get; set; } = null!;

    /// <summary>
    /// 로그인 ID
    /// </summary>
    public string? LoginId { get; set; }

    /// <summary>
    /// 사용자 이름
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// 대문자로 변환된 사용자 이름
    /// </summary>
    public string? NormalizedUserName { get; set; }

    /// <summary>
    /// 이메일 주소
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// 대문자로 변환된 이메일 주소
    /// </summary>
    public string? NormalizedEmail { get; set; }

    /// <summary>
    /// 이메일 인증 여부
    /// </summary>
    public bool EmailConfirmed { get; set; }

    /// <summary>
    /// 비밀번호 해시
    /// </summary>
    public string? PasswordHash { get; set; }

    /// <summary>
    /// 보안 스탬프
    /// </summary>
    public string? SecurityStamp { get; set; }

    /// <summary>
    /// 동시성 제어 스탬프
    /// </summary>
    public string? ConcurrencyStamp { get; set; }

    /// <summary>
    /// 전화번호
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// 전화번호 인증 여부
    /// </summary>
    public bool PhoneNumberConfirmed { get; set; }

    /// <summary>
    /// 2단계 인증 활성화 여부
    /// </summary>
    public bool TwoFactorEnabled { get; set; }

    /// <summary>
    /// 잠금 해제 시간
    /// </summary>
    public DateTime? LockoutEnd { get; set; }

    /// <summary>
    /// 계정 잠금 가능 여부
    /// </summary>
    public bool LockoutEnabled { get; set; }

    /// <summary>
    /// 로그인 실패 횟수
    /// </summary>
    public int AccessFailedCount { get; set; }

    public virtual ICollection<UserLogin> UserLogins { get; set; } = new List<UserLogin>();

    public virtual ICollection<UserToken> UserTokens { get; set; } = new List<UserToken>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
    
}
