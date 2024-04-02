using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    private readonly string[] _claimValues;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="claimType">Claim 타입</param>
    /// <param name="claimValues">Claim 값</param>
    public ClaimRequirementFilter(string claimType, string claimValues)
    {
        _claimType = claimType;
        _claimValues = claimValues.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(value => value.Trim()).ToArray();
    }

    /// <summary>
    /// 액션이 실행 될때
    /// </summary>
    /// <param name="context">액션 Context</param>
    /// <param name="next">Delegate</param>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // [AllowAnonymous] 체크
        if (context.ActionDescriptor.EndpointMetadata.Any(m => m is AllowAnonymousAttribute))
        {
            await next(); // 다음 미들웨어로 진행
            return;
        }
        
        // 로그인이 되어있지 않을경우 
        if (context.HttpContext.User.Identity is {IsAuthenticated: false})
        {
            // 로그인 되어 있지 않음
            var response = context.HttpContext.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.Unauthorized; // 401 Unauthorized

            var result = JsonSerializer.Serialize(new
            {
                Code = "UNAUTHORIZED",
                Message = "로그인후 이용해주세요",
                IsAuthenticated = false,
                Result = "Error"
            });

            await response.WriteAsync(result);
            return;
        }
        
        // 권한 체크
        bool hasClaim = context.HttpContext.User.Claims.Any(userClaim => 
            userClaim.Type == _claimType && 
            _claimValues.Any(allowedValue => 
                string.Equals(userClaim.Value, allowedValue, StringComparison.OrdinalIgnoreCase)));
        
        // Claim 이 존재하지 않을 경우
        if (!hasClaim)
        {
            var response = context.HttpContext.Response;
            response.StatusCode = (int)HttpStatusCode.NotAcceptable; 
            return;
        }

        // 권한 있음, 다음 미들웨어/액션으로 진행
        await next();
    }
}