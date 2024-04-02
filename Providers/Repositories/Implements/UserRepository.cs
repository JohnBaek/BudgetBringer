using Features.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.DataModels;
using Providers.Repositories.Interfaces;

namespace Providers.Repositories.Implements;


/// <summary>
/// 유저 정보 리파지토리 인터페이스
/// </summary>
public class UserRepository : IUserRepository
{
    /// <summary>
    /// DB Context
    /// </summary>
    private readonly AnalysisDbContext _dbContext;

    /// <summary>
    /// 로거
    /// </summary>
    private readonly ILogger<UserRepository> _logger;

    /// <summary>
    /// 사용자 매니저
    /// </summary>
    private readonly UserManager<DbModelUser> _userManager;
    
    
    /// <summary>
    /// IHttpContextAccessor
    /// </summary>
    private readonly IHttpContextAccessor _httpContextAccessor;


    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="dbContext">DB Context</param>
    /// <param name="logger">로거</param>
    /// <param name="userManager"></param>
    /// <param name="httpContextAccessor">IHttpContextAccessor</param>
    public UserRepository(
          AnalysisDbContext dbContext
        , ILogger<UserRepository> logger
        , UserManager<DbModelUser> userManager
        , IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _logger = logger;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }
    
    /// <summary>
    /// 사용자의 아이디와 패스워드로 사용자가 존재하는지 확인한다.
    /// </summary>
    /// <param name="loginId">로그인 아이디</param>
    /// <returns>결과</returns>
    public async Task<bool> ExistUserAsync(string loginId)
    {
        try
        {
            return await _dbContext.Users.AsNoTracking().AnyAsync(i => i.LoginId == loginId);
        }
        catch (Exception e)
        {
            e.LogError(_logger);
        }
    
        return false;
    }
    
    /// <summary>
    /// 사용자의 아이디와 패스워드로 사용자를 가져온다.
    /// </summary>
    /// <param name="loginId">로그인 아이디</param>
    /// <param name="password">패스워드 (SHA 256 인크립트 된 원본)</param>
    /// <returns>결과</returns>
    public async Task<DbModelUser?> GetUserWithIdPasswordAsync(string loginId, string password)
    {
        DbModelUser? result;
    
        try
        {
            // 사용자의 정보를 찾는다.
            DbModelUser? findUser = await _dbContext.Users.AsNoTracking()
                .Where(i => i.LoginId == loginId).FirstOrDefaultAsync();
                
            // 찾을수 없는경우 
            if (findUser == null)
                return null;
            
            return findUser;
        }
        catch (Exception e)
        {
            result = null;
            e.LogError(_logger);
        }
    
        return result;
    }


    /// <summary>
    /// 사용자의 Id 값으로 권한 값의 리스트를 가져온다.
    /// </summary>
    /// <param name="userId">사용자 아이디</param>
    /// <returns></returns>
    public async Task<List<string>> GetRolesByUserAsync(Guid userId)
    {
        List<string> result = new List<string>();
    
        try
        {
            result = (await _dbContext.Roles.AsNoTracking()
                .Join(_dbContext.UserRoles.Where(i => i.UserId == userId)
                    , role => role.Id
                    , userRole => userRole.RoleId
                    , (role, userRole)
                        => role.Name
                    )
                .ToListAsync())!;
        }
        catch (Exception e)
        {
            e.LogError(_logger);
        }
    
        return result;
    }

    /// <summary>
    /// 로그인한 사용자의 정보를 가져온다.
    /// </summary>
    /// <returns></returns>
    public async Task<DbModelUser> GetAuthenticatedUser()
    {
        DbModelUser? result;
    
        try
        {
            // 세션이 비어있을 경우 
            if (_httpContextAccessor.HttpContext?.User == null)
                return new DbModelUser();
            
            result = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
        }
        catch (Exception e)
        {
            result = null;
            e.LogError(_logger);
        }
    
        return result;
    }
}