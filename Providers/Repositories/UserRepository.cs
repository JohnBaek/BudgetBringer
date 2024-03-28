using Features.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.DataModels;

namespace Providers.Repositories;


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
    /// 사이닝 매니저
    /// </summary>
    private readonly SignInManager<IdentityUser> _signInManager;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="dbContext">DB Context</param>
    /// <param name="logger">로거</param>
    /// <param name="signInManager">사이닝 매니저</param>
    public UserRepository(AnalysisDbContext dbContext, ILogger<UserRepository> logger, SignInManager<IdentityUser> signInManager)
    {
        _dbContext = dbContext;
        _logger = logger;
        _signInManager = signInManager;
    }
    
    /// <summary>
    /// 사용자의 아이디와 패스워드로 사용자가 존재하는지 확인한다.
    /// </summary>
    /// <param name="loginId">로그인 아이디</param>
    /// <returns>결과</returns>
    public async Task<bool> ExistUserAsync(string loginId)
    {
        bool result;
        try
        {
            // return await _dbContext.Users.AsNoTracking().AnyAsync(i => i.LoginId == loginId);
        }
        catch (Exception e)
        {
            result = false;
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
    public async Task<IdentityUser?> GetUserWithIdPasswordAsync(string loginId, string password)
    {
        IdentityUser? result;
    
        try
        {
            result = await _dbContext.Users.AsNoTracking()
                .Where(i => i.UserName == loginId && i.PasswordHash == password)
                .FirstOrDefaultAsync();
    
            // 찾을수 없는경우 
            if (result == null)
                return null;
    
            // 사용자를 로그인시킨다.
            await _signInManager.SignInAsync(result, isPersistent: false);
        }
        catch (Exception e)
        {
            result = null;
            e.LogError(_logger);
        }
    
        return result;
    }
    
}