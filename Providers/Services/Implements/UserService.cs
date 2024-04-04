using System.Security.Claims;
using Features.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Models.Common.Enums;
using Models.DataModels;
using Models.Responses;
using Models.Responses.Users;
using Providers.Repositories;
using Providers.Repositories.Interfaces;
using Providers.Services.Interfaces;
using Exception = System.Exception;

namespace Providers.Services.Implements;

/// <summary>
/// 사용자 서비스 구현체
/// </summary>
public class UserService : IUserService 
{
    /// <summary>
    /// 로거
    /// </summary>
    private readonly ILogger<UserService> _logger;

    /// <summary>
    /// 유저 매니저
    /// </summary>
    private readonly UserManager<DbModelUser> _userManager;

    /// <summary>
    /// 역할 매니저
    /// </summary>
    private readonly RoleManager<DbModelRole> _roleManager;
    
    /// <summary>
    /// IHttpContextAccessor
    /// </summary>
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="logger">로거</param>
    /// <param name="userManager">사용자 매니저</param>
    /// <param name="roleManager">역할 매니저</param>
    /// <param name="httpContextAccessor">IHttpContextAccessor</param>
    public UserService( 
        ILogger<UserService> logger
        , UserManager<DbModelUser> userManager
        , RoleManager<DbModelRole> roleManager
        , IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
        _httpContextAccessor = httpContextAccessor;
    }
    
    /// <summary>
    /// 로그인한 사용자의 권한 목록을 가져온다.
    /// </summary>
    /// <returns></returns>
    public async Task<ResponseList<ResponseUserRole>> GetRolesByUserAsync()
    {
        ResponseList<ResponseUserRole> result = new ResponseList<ResponseUserRole>();
    
        try
        {
            // 세션이 비어있을 경우 
            if (_httpContextAccessor.HttpContext?.User == null)
                return result;
            
            DbModelUser? user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            // 세션에 사용자 정보가 없는경우 
            if (user == null)
                return result;

            // 반환할 유저 역할정보
            List<ResponseUserRole> roles = new List<ResponseUserRole>();
            
            // 로그인한 사용자의 전체 DbModelRole 을 조회한다.
            IList<string> userRoles = await _userManager.GetRolesAsync(user);
            
            // 사용자의 전체 DbModelRole 에 대해 처리한다.
            foreach (string userRole in userRoles)
            {
                ResponseUserRole add = new ResponseUserRole
                {
                    Name = userRole ,
                    Claims = new List<ResponseUserRoleClaim>()
                };
                var role = await _roleManager.FindByNameAsync(userRole);
                
                // 찾을수 없을경우 
                if(role == null)
                    continue;
                
                // 역할 Claim 을 찾는다.
                IList<Claim> roleClaims = await _roleManager.GetClaimsAsync(role);
                IList<Claim> userDefinedClaim = await _userManager.GetClaimsAsync(user);
                
                // 사용자 정의 Claim 에 대해 처리
                foreach (Claim claim in userDefinedClaim)
                {
                    roleClaims.Add(claim);         
                }
                
                // 모든 역할 Claim 에 대해 처리
                foreach (Claim claim in roleClaims)
                {
                    ResponseUserRoleClaim addClaim = new ResponseUserRoleClaim
                    {   
                        Type = claim.Type ,
                        Value = claim.Value
                    };
                    add.Claims.Add(addClaim);
                }
                
                roles.Add(add);
            }
            
            result.Items = roles;
            result.Result = EnumResponseResult.Success;
        }
        catch (Exception e)
        {
            result = new ResponseList<ResponseUserRole>{ Code = "ERR", Message = "처리중 예외가 발생했습니다." };
            e.LogError(_logger);
        }
    
        return result;
    }
}