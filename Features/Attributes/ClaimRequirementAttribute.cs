namespace Features.Attributes;

/// <summary>
/// Claim 기반 Authorize 애트리뷰트
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class ClaimRequirementAttribute : Attribute
{
    /// <summary>
    /// Claim 타입
    /// </summary>
    public string ClaimType { get; }
    
    /// <summary>
    /// Claim 값
    /// </summary>
    public string ClaimValue { get; }

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="claimType"></param>
    /// <param name="claimValue"></param>
    public ClaimRequirementAttribute(string claimType, string claimValue)
    {
        ClaimType = claimType;
        ClaimValue = claimValue;
    }
}