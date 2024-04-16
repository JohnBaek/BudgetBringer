using System.Net;
using System.Text.Json;
using Models.Common.Enums;
using Models.Responses;

namespace Apis.Middlewares;

/// <summary>
/// 인증 미들웨어 
/// </summary>
public class HandleUnauthorizedMiddleware
{
    /// <summary>
    /// 요청 델리게이터
    /// </summary>
    private readonly RequestDelegate _requestDelegate;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="requestDelegate">요청 델리게이터</param>
    public HandleUnauthorizedMiddleware(RequestDelegate requestDelegate)
    {
        _requestDelegate = requestDelegate;
    }
    
    /// <summary>
    /// 응답 정보를 핸들링한다.
    /// </summary>
    /// <param name="httpContext">HttpContext</param>
    public async Task Invoke(HttpContext httpContext)
    {
        // 파이프라인의 다음 미들웨어 실행
        await _requestDelegate(httpContext);
        
        // 정상적인경우 
        if (httpContext.Response.StatusCode == (int) HttpStatusCode.OK ||
            httpContext.Response.StatusCode == (int) HttpStatusCode.Accepted)
        {
            await _requestDelegate(httpContext);
        }
        // 인증되지 않은 사용자 인경우 
        else if (httpContext.Response.StatusCode == (int)HttpStatusCode.Unauthorized || httpContext.Response.StatusCode == (int)HttpStatusCode.NotFound)
        {
            httpContext.Response.StatusCode = (int) HttpStatusCode.OK;
            httpContext.Response.ContentType = "application/json";
            Response response = new Response
            {
                Code = "UNAUTHORIZED",
                Message = $"로그인 후 이용해주세요 [{ httpContext.Response.StatusCode }]",
                IsAuthenticated = false,
                Result = EnumResponseResult.Error
            };

            string responseJson = JsonSerializer.Serialize(response);
            await httpContext.Response.WriteAsync(responseJson);
        }
        // 권한이 없는경우
        else if (httpContext.Response.StatusCode == (int) HttpStatusCode.Forbidden ||
                 httpContext.Response.StatusCode == (int) HttpStatusCode.NotAcceptable ||
                 httpContext.Response.StatusCode == (int) HttpStatusCode.Found 
                 )
        {

            httpContext.Response.StatusCode = (int) HttpStatusCode.OK;
            httpContext.Response.ContentType = "application/json";
            Response response = new Response
            {
                Code = "",
                Message = "접근 권한이 없습니다.",
                IsAuthenticated = false,
                Result = EnumResponseResult.Error
            };

            string responseJson = JsonSerializer.Serialize(response);
            await httpContext.Response.WriteAsync(responseJson);
        }
    }
}

/// <summary>
/// 401 인증되지 않은 요청에 대한 핸들링 확장 메서드
/// </summary>
public static class HandleUnauthorizedExtensions
{
    /// <summary>
    /// 미들웨어 사용 확장메서드
    /// </summary>
    /// <param name="builder">IApplicationBuilder</param>
    /// <returns></returns>
    public static IApplicationBuilder UseHandleUnauthorized(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<HandleUnauthorizedMiddleware>();
    }
}