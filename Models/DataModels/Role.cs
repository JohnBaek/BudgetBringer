using System;
using System.Collections.Generic;

namespace Models.DataModels;

public partial class Role
{
    /// <summary>
    /// 역할의 고유 식별자
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 역할 이름
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 역할 이름의 정규화된 형태
    /// </summary>
    public string? NormalizedName { get; set; }

    /// <summary>
    /// 병행 처리를 위한 스탬프
    /// </summary>
    public string? ConcurrencyStamp { get; set; }

    public virtual ICollection<RoleClaim> RoleClaims { get; set; } = new List<RoleClaim>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
