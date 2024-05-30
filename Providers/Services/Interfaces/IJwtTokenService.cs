using System.Security.Claims;
using Models.Responses;
using Models.Responses.Authentication;

namespace Providers.Services.Interfaces;

/// <summary>
/// JWT token service
/// </summary>
public interface IJwtTokenService
{
    /// <summary>
    /// Generate Token
    /// </summary>
    /// <param name="loginId"></param>
    /// <param name="expiredMinutes"></param>
    /// <returns></returns>
    Task<ResponseData<ResponseToken>> Generate(string loginId, int expiredMinutes = 20);

    /// <summary>
    /// Generate Token
    /// </summary>
    /// <param name="loginId">loginId</param>
    /// <param name="expiredMinutes">expiredMinutes</param>
    /// <returns></returns>
    Task<string> GenerateTokenAsync(string loginId, int expiredMinutes = 20);

    /// <summary>
    /// Generate refresh token
    /// </summary>
    /// <returns></returns>
    string GenerateRefreshToken();

    /// <summary>
    /// Validate Token
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    ResponseData<ClaimsPrincipal> GetPrincipalFromToken(string token);
}