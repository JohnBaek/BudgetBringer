using Features.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Features.Attributes;

/// <summary>
/// Claim 기반 Authorize 애트리뷰트
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class ClaimRequirementAttribute : TypeFilterAttribute
{
    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="claimType">타입</param>
    /// <param name="claimValue">값</param>
    public ClaimRequirementAttribute(string claimType, string claimValue)
        : base(typeof(ClaimRequirementFilter))
    {
        Arguments = new object[] { claimType, claimValue };
    }
}