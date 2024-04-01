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
    private readonly ILogger<AuthenticationService> _logger;
    
    /// <summary>
    /// 사용자 리파지토리
    /// </summary>
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// 사인인 매니저 
    /// </summary>
    private readonly ISignInService<DbModelUser> _signInService;

    /// <summary>
    /// 유저 매니저
    /// </summary>
    private readonly UserManager<DbModelUser> _userManager;

    /// <summary>
    /// 역할 매니저
    /// </summary>
    private readonly RoleManager<DbModelRole> _roleManager;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="logger">로거</param>
    /// <param name="userRepository">사용자 리파지토리</param>
    /// <param name="signInService">사인인 서비스</param>
    /// <param name="userManager">사용자 매니저</param>
    /// <param name="roleManager">역할 매니저</param>
    public UserService( 
        ILogger<AuthenticationService> logger
        , IUserRepository userRepository
        , ISignInService<DbModelUser> signInService
        , UserManager<DbModelUser> userManager
        , RoleManager<DbModelRole> roleManager)
    {
        _logger = logger;
        _userRepository = userRepository;
        _signInService = signInService;
        _userManager = userManager;
        _roleManager = roleManager;
    }
    
    /// <summary>
    /// 로그인한 사용자의 권한 목록을 가져온다.
    /// </summary>
    /// <returns></returns>
    public async Task<ResponseList<ResponseUserRole>> GetRolesByUserAsync(HttpContext httpContext)
    {
        ResponseList<ResponseUserRole> result = new ResponseList<ResponseUserRole>();
    
        try
        {
            DbModelUser? user = await _userManager.GetUserAsync(httpContext.User);

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
    //
    // /// <summary>
    // /// 로그인한 사용자의 Claim 목록을 가져온다.
    // /// </summary>
    // /// <param name="httpContext">HttpContext</param>
    // /// <returns></returns>
    // public async Task<ResponseList<ResponseUserClaim>> GetClaimByUserAsync(HttpContext httpContext)
    // {
    //     ResponseList<ResponseUserClaim> result = new ResponseList<ResponseUserClaim>();
    //
    //     try
    //     {
    //         DbModelUser? dbModelUser = await _userManager.GetUserAsync(httpContext.DbModelUser);
    //
    //         // 세션에 사용자 정보가 없는경우 
    //         if (dbModelUser == null)
    //             return result;
    //
    //         // 사용자의 모든 Claim을 가져온다.
    //         IList<Claim> claims = await _userManager.GetClaimsAsync(dbModelUser);
    //
    //         // 반환할 사용자의 claim 정보 
    //         List<ResponseUserClaim> items = new List<ResponseUserClaim>();
    //
    //         // 모든 Claim 에 대해 처리한다.
    //         foreach (Claim claim in claims)
    //         {
    //             items.Add(new ResponseUserClaim
    //             {
    //                 Name = claim.Type ,
    //                 Value = claim.Value,
    //             });
    //         }
    //
    //         result.Items = items;
    //         result.Result = EnumResponseResult.Success;
    //     }
    //     catch (Exception e)
    //     {
    //         result = new ResponseList<ResponseUserClaim>{ Code = "ERR", Message = "처리중 예외가 발생했습니다." };
    //         e.LogError(_logger);
    //     }
    //
    //     return result;
    // }
}