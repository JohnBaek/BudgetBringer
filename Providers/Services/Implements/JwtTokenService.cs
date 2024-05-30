using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Features.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Models.Common.Enums;
using Models.DataModels;
using Models.Responses;
using Models.Responses.Authentication;
using Providers.Services.Interfaces;

namespace Providers.Services.Implements;

/// <summary>
/// JWT token service
/// </summary>
public class JwtTokenService : IJwtTokenService
{
    /// <summary>
    /// Configurations
    /// </summary>
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Logger
    /// </summary>
    private readonly ILogger<JwtTokenService> _logger;

    /// <summary>
    /// DB Context
    /// </summary>
    private readonly AnalysisDbContext _dbContext;

    /// <summary>
    /// User Manager
    /// </summary>
    private readonly UserManager<DbModelUser> _userManager;

    /// <summary>
    /// JWT Secret 키
    /// </summary>
    private readonly SecurityKey _securityKey;

    /// <summary>
    /// JWT Issuer
    /// </summary>
    private readonly SigningCredentials _signingCredentials;

    /// <summary>
    /// _jwtSecret
    /// </summary>
    private readonly string _jwtSecret;

    /// <summary>
    /// _jwtIssuer
    /// </summary>
    private readonly string _jwtIssuer;


    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="dbContext"></param>
    /// <param name="configuration"></param>
    /// <param name="userManager"></param>
    public JwtTokenService(
          ILogger<JwtTokenService> logger
        , AnalysisDbContext dbContext
        , IConfiguration configuration
        , UserManager<DbModelUser> userManager)
    {
        _logger = logger;
        _dbContext = dbContext;
        _configuration = configuration;
        _userManager = userManager;

        // Set JWT Credentials
        _jwtSecret = _configuration["jwt:Secret"] ?? "";
        _jwtIssuer = _configuration["jwt:ValidIssuer"] ?? "";
        _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
        _signingCredentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);
    }

    /// <summary>
    /// GenerateAsync Token and Refresh Token
    /// </summary>
    /// <param name="loginId"></param>
    /// <param name="expiredMinutes"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<ResponseData<ResponseToken>> GenerateAsync(string loginId, int expiredMinutes = 20)
    {
        ResponseData<ResponseToken> result;
        try
        {
            string token = await GenerateTokenAsync(loginId, expiredMinutes: expiredMinutes);
            string refreshToken = GenerateRefreshToken();
            return new ResponseData<ResponseToken>(EnumResponseResult.Success, "", "", new ResponseToken()
            {
                Token = token,
                RefreshToken = refreshToken,
            });
        }
        catch (Exception e)
        {
            e.LogError(_logger);
            result = new ResponseData<ResponseToken>(EnumResponseResult.Error, "", "", null);
        }
        return result;
    }

    /// <summary>
    /// GenerateAsync Token
    /// </summary>
    /// <param name="loginId">loginId</param>
    /// <param name="expiredMinutes">expiredMinutes</param>
    /// <returns></returns>
    public async Task<string> GenerateTokenAsync(string loginId, int expiredMinutes = 20)
    {
        string result = "";
        try
        {
            // Get User by loginId
            DbModelUser? user = await _dbContext.Users.Where(i => i.LoginId == loginId).FirstOrDefaultAsync();

            // If Cannot find user
            if (user == null)
                throw new Exception();

            List<Claim> claims = (await _userManager.GetClaimsAsync(user: user)).ToList();

            // GenerateAsync Token
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                _jwtIssuer, _jwtIssuer,
                claims, expires: DateTime.Now.AddMinutes(expiredMinutes),
                signingCredentials: _signingCredentials
            );

            result = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
        catch (Exception e)
        {
            e.LogError(_logger);
        }
        return result;
    }

    /// <summary>
    /// GenerateAsync refresh token Base64
    /// </summary>
    /// <returns></returns>
    public string GenerateRefreshToken()
    {
        string result = "";
        try
        {
            byte[] randomNumber = new byte[32];
            using var randomNumberGenerator = System.Security.Cryptography.RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        catch (Exception e)
        {
            e.LogError(_logger);
        }
        return result;
    }



    /// <summary>
    /// Validate Token
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public ResponseData<ClaimsPrincipal> GetPrincipalFromToken(string token)
    {
        ResponseData<ClaimsPrincipal> result;
        try
        {
            // Signing Key
            SymmetricSecurityKey signKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(_configuration["jwt:Secret"] ?? ""));

            // Create Token Validation Parameters
            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signKey,
                ClockSkew = TimeSpan.Zero
            };

            // Validate and Get principal
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            // Failed Validate
            if (!(securityToken is JwtSecurityToken jwtSecurityToken) ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return new ResponseData<ClaimsPrincipal>(EnumResponseResult.Error, "Invalid Token", "Invalid Token", null);
            }
            return new ResponseData<ClaimsPrincipal>(EnumResponseResult.Success, "", "", principal);
        }
        catch (Exception e)
        {
            e.LogError(_logger);
            result = new ResponseData<ClaimsPrincipal>(EnumResponseResult.Error, "", e.Message, null);
        }
        return result;
    }
}