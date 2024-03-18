using BasicAuthenticated.Resources;
using Microsoft.AspNetCore.Identity;

namespace BasicAuthentication.Repositories;

public interface IUserRepository
{
    Task<User> FindByLoginIdAsync(string loginId);
    Task<SignInResult> CheckPasswordSignInAsync(User user, string password);
}