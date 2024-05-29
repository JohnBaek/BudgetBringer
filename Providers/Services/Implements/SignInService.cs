using Features.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models.Common.Enums;
using Models.DataModels;
using Models.Responses;
using Providers.Services.Interfaces;

namespace Providers.Services.Implements;

/// <summary>
/// SignInManager 래핑 클래스 구현체
/// </summary>
public class SignInService : ISignInService<DbModelUser>
{
    /// <summary>
    /// 사인인 매니저
    /// </summary>
    private readonly SignInManager<DbModelUser> _signInManager;
    
    /// <summary>
    /// IHttpContextAccessor
    /// </summary>
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// HashService
    /// </summary>
    private readonly IHashService _hashService;

    /// <summary>
    /// DB Context
    /// </summary>
    private readonly AnalysisDbContext _dbContext;

    /// <summary>
    /// UserManager
    /// </summary>
    private readonly UserManager<DbModelUser> _userManager;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="signInManager">사인인 매니저</param>
    /// <param name="httpContextAccessor">IHttpContextAccessor</param>
    /// <param name="hashService"></param>
    /// <param name="dbContext"></param>
    public SignInService(SignInManager<DbModelUser> signInManager, IHttpContextAccessor httpContextAccessor, IHashService hashService, AnalysisDbContext dbContext)
    {
        _signInManager = signInManager;
        _httpContextAccessor = httpContextAccessor;
        _hashService = hashService;
        _dbContext = dbContext;
    }


    /// <summary>
    /// 사인 아웃
    /// </summary>
    /// <returns></returns>
    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    /// <summary>
    /// 유저정보로 강제로그인
    /// </summary>
    /// <param name="dbModelUser">The dbModelUser to sign-in.</param>
    /// <param name="isPersistent">Flag indicating whether the sign-in cookie should persist after the browser is closed.</param>
    /// <param name="authenticationMethod">DisplayName of the method used to authenticate the dbModelUser.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task SignInAsync(DbModelUser dbModelUser, bool isPersistent, string? authenticationMethod = null)
    {
        await _signInManager.SignInAsync(dbModelUser, isPersistent, authenticationMethod);
    }

    /// <summary>
    /// 로그인여부를 반환한다.
    /// </summary>
    /// <returns></returns>
    public Response IsSignedIn()
    {
        Response response = new Response{ IsAuthenticated = false };

        // 세션이 비어있을 경우
        if (_httpContextAccessor.HttpContext?.User == null)
            return response;
        
        // 로그인 여부를 가져온다.
        bool isAuthenticated = _signInManager.IsSignedIn(_httpContextAccessor.HttpContext.User);

        // 인증되어있는 경우 
        if (isAuthenticated)
            return new Response {Result = EnumResponseResult.Success, IsAuthenticated = true};
        
        return response;
    }

    /// <summary>
    /// 패스워드 정보로 로그인 시킨다.
    /// </summary>
    /// <param name="loginId"></param>
    /// <param name="password"></param>
    /// <param name="isPersistent"></param>
    /// <param name="lockoutOnFailure"></param>
    /// <returns></returns>
    public async Task<Response> PasswordSignInAsync(string loginId, string password, bool isPersistent, bool lockoutOnFailure)
    {
        // Hashing password
        string hashPassword = _hashService.ComputeHash(password);

        // Is matches Id and password?
        DbModelUser? user = await _dbContext.Users.Where(i => i.PasswordHash == hashPassword && i.LoginId == loginId).FirstOrDefaultAsync();

        // Cannot find user
        if (user == null)
            return new Response(EnumResponseResult.Error, "아이디 또는 비밀번호를 확인해주세요", "");

        await _signInManager.SignInAsync(user, isPersistent: false);
        return new Response(EnumResponseResult.Success,"","");
    }
}