using BasicAuthenticated.Resources;
using BasicAuthentication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BasicAuthentication.Repositories;

public class UserRepository : IUserRepository
{
    private readonly BasicAuthenticatedDbContext _context;
    private readonly SignInManager<User> _signInManager;

    public UserRepository(BasicAuthenticatedDbContext context, SignInManager<User> signInManager)
    {
        _context = context;
        _signInManager = signInManager;
    }

    public async Task<User> FindByLoginIdAsync(string loginId)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.LoginId == loginId);
    }

    public async Task<SignInResult> CheckPasswordSignInAsync(User user, string password)
    {
        // 주의: 이 메서드는 비밀번호를 검증하고 SignInManager의 로그인 시도 결과를 반환합니다.
        // 실제 환경에서는 사용자의 로그인 시도를 추적하기 위해 SignInManager를 사용할 수 있지만,
        // 여기서는 단지 비밀번호 검증의 예를 들기 위해 사용합니다.
        // 실제 구현에서는 비밀번호 해싱 및 검증 로직을 직접 처리할 수도 있습니다.
        var result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false);
        return result;
    }
}