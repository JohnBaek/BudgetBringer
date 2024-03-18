using System.Threading.Tasks;
using BasicAuthenticated.Resources;
using BasicAuthentication.Providers;
using BasicAuthentication.Repositories;
using BasicAuthentication.ViewModels.Enums;
using BasicAuthentication.ViewModels.Requests.Login;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace BasicAuthentication.Tests.Providers;

[TestSubject(typeof(LoginProvider))]
public class LoginProviderTests
{
    private readonly Mock<IUserRepository> _mockUserRepository = new Mock<IUserRepository>();
    private readonly LoginProvider _loginProvider;

    public LoginProviderTests()
    {
        _loginProvider = new LoginProvider(_mockUserRepository.Object);
    }

    [Fact]
    public async Task TryLogin_UserNotFound_ReturnsErrorMessage()
    {
        var request = new RequestLogin { LoginId = "nonexistentUser", Password = "password" };
        _mockUserRepository.Setup(repo => repo.FindByLoginIdAsync(request.LoginId)).ReturnsAsync((User)null);

        var result = await _loginProvider.TryLogin(request);

        Assert.Equal("사용자 정보를 찾을 수 없거나 비밀번호가 일치하지 않습니다.", result.Message);
    }

    [Fact]
    public async Task TryLogin_InvalidPassword_ReturnsErrorMessage()
    {
        var user = new User { LoginId = "validUser", PasswordHash = "hashedPassword" };
        _mockUserRepository.Setup(repo => repo.FindByLoginIdAsync(user.LoginId)).ReturnsAsync(user);
        _mockUserRepository.Setup(repo => repo.CheckPasswordSignInAsync(user, It.IsAny<string>())).ReturnsAsync(SignInResult.Failed);

        var result = await _loginProvider.TryLogin(new RequestLogin { LoginId = user.LoginId, Password = "invalidPassword" });

        Assert.Equal("사용자 정보를 찾을 수 없거나 비밀번호가 일치하지 않습니다.", result.Message);
    }

    [Fact]
    public async Task TryLogin_ValidCredentials_ReturnsSuccess()
    {
        var user = new User { LoginId = "validUser", PasswordHash = "hashedPassword" };
        _mockUserRepository.Setup(repo => repo.FindByLoginIdAsync(user.LoginId)).ReturnsAsync(user);
        _mockUserRepository.Setup(repo => repo.CheckPasswordSignInAsync(user, It.IsAny<string>())).ReturnsAsync(SignInResult.Success);

        var result = await _loginProvider.TryLogin(new RequestLogin { LoginId = user.LoginId, Password = "correctPassword" });

        Assert.Equal(EnumResponseResult.Success, result.Result);
    }
}