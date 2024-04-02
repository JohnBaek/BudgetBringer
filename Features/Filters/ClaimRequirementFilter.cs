using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Features.Filters;

/// <summary>
/// Claim 기반 필터
/// </summary>
public class ClaimRequirementFilter : IAsyncActionFilter
{
    /// <summary>
    /// Claim 타입
    /// </summary>
    private readonly string _claimType;
    
    /// <summary>
    /// Claim 값
    /// </summary>
    private readonly string _claimValue;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="claimType">Claim 타입</param>
    /// <param name="claimValue">Claim 값</param>
    public ClaimRequirementFilter(string claimType, string claimValue)
    {
        _claimType = claimType;
        _claimValue = claimValue;
    }

    /// <summary>
    /// 액션이 실행 될때
    /// </summary>
    /// <param name="context">액션 Context</param>
    /// <param name="next">Delegate</param>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // 사용자가 가진 Claim 과 메서드에서 요구하는 Claim 이 일치 할경우 
        bool hasClaim = context.HttpContext.User.Claims.Any(c => 
            c.Type == _claimType && 
            c.Value == _claimValue);
        
        // Claim 이 존재하지 않을경우 
        if (!hasClaim)
        {
            context.Result = new ForbidResult();
            return;
        }

        await next();
    }
}