using BasicAuthenticated.Resources;
using BasicAuthentication.Interfaces;
using BasicAuthentication.Models;
using BasicAuthentication.ViewModels.Enums;
using BasicAuthentication.ViewModels.Requests.Login;
using BasicAuthentication.ViewModels.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BasicAuthentication.Providers;

/// <summary>
/// 로그인 프로바이더
/// </summary>
public class LoginProvider : ILoginService
{
    /// <summary>
    /// SHA256 해싱 
    /// </summary>
    private readonly IPasswordHasher<User> _passwordHasher;
    
    /// <summary>
    /// 디비 컨텍스트
    /// </summary>
    private readonly BasicAuthenticatedDbContext _dbContext;

    /// <summary>
    /// Authentication 매니저 
    /// </summary>
    private readonly SignInManager<User> _signInManager;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="dbContext">디비 컨텍스트</param>
    /// <param name="signInManager">로그인 인증 관련 매니저</param>
    /// <param name="passwordHasher">패스워드 해싱</param>
    public LoginProvider(
        BasicAuthenticatedDbContext dbContext, 
        SignInManager<User> signInManager, 
        IPasswordHasher<User> passwordHasher)
    {
        _dbContext = dbContext;
        _signInManager = signInManager;
        _passwordHasher = passwordHasher;
    }
    
    /// <summary>
    /// 로그인을 시도한다.
    /// </summary>
    /// <param name="request">로그인 요청정보</param>
    /// <returns>결과</returns>
    public async Task<Response> TryLogin(RequestLogin request)
    {
        Response result;
        
        try
        {
            // 사용자 정보를 찾는다.
            User? findUser = await _dbContext.Users.AsNoTracking().Where(user => user.LoginId == request.LoginId).FirstOrDefaultAsync();
            
            // 사용자가 정보가 없는 경우 
            if (findUser == null)
                return new Response("", "사용자 정보를 찾을수 없거나 비밀번호가 일치하지 않습니다.");

            // 사용자 정보로 로그인을 시도한다.
            SignInResult signInResult = await _signInManager.PasswordSignInAsync(findUser, _passwordHasher.HashPassword(new User() ,request.Password), false, false);
            
            // 계정이 잠겨있는 경우
            if(signInResult.IsLockedOut)
                return new Response("", "사용자의 계정이 잠겨있습니다 관리자에게 문의하세요");
            
            // 접근이 허용되지 않는경우
            if(signInResult.IsNotAllowed)
                return new Response("", "접근이 허용되지 않습니다.");

            // 로그인 성공
            return new Response(EnumResponseResult.Success, "", "");
        }
        catch (Exception e)
        {
            result = new Response("", "알수없는 문제가 발생했습니다.");
        }

        return result;
    }
}