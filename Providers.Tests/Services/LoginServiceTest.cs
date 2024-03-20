using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Models.Common.Enums;
using Models.DataModels;
using Models.Requests.Login;
using Moq;
using Providers.Repositories;
using Providers.Services;
using Xunit;

namespace Providers.Tests.Services;

/// <summary>
/// 로그인 서비스 테스트 클래스
/// </summary>
[TestSubject(typeof(LoginService))]
public class LoginServiceTest
{
    /// <summary>
    /// 유저 리파지토리 
    /// </summary>
    private readonly Mock<IUserRepository> _mockUserRepository;
    
    /// <summary>
    /// 로그인서비스 
    /// </summary>
    private readonly LoginService _loginService;

    /// <summary>
    /// 생성자
    /// </summary>
    public LoginServiceTest()
    {
        // Arranges
        var mockLogger = new Mock<ILogger<LoginService>>();
        _mockUserRepository = new Mock<IUserRepository>();
        _loginService = new LoginService(mockLogger.Object, _mockUserRepository.Object);
    }

    /// <summary>
    /// 로그인 성공
    /// </summary>
    [Fact]
    public async Task TryLoginAsync_Success()
    {
        // 요청정보 세팅
        var request = new RequestLogin
        {
            LoginId = "testUser",
            Password = "password"
        };

        _mockUserRepository.Setup(x => x.ExistUserAsync(It.IsAny<string>())).ReturnsAsync(true);
        _mockUserRepository.Setup(x => x.GetUserWithIdPasswordAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new User());

        // Act
        var result = await _loginService.TryLoginAsync(request);

        // Assert
        result.Result.Should().Be(EnumResponseResult.Success);
    }
    
    /// <summary>
    /// 사용자를 찾을수 없을때
    /// </summary>
    [Fact]
    public async Task TryLoginAsync_Fail_UserNotFound()
    {
        var request = new RequestLogin
        {
            LoginId = "nonExistingUser",
            Password = "password"
        };

        _mockUserRepository.Setup(x => x.ExistUserAsync(It.IsAny<string>())).ReturnsAsync(false);

        // Act
        var result = await _loginService.TryLoginAsync(request);

        // Assert
        result.Data.Should().BeNull();
        result.Code.Should().Be("ERR");
        result.Message.Should().Be("사용자를 찾지 못했습니다.");
    }
    
    /// <summary>
    /// 비밀번호 아이디가 틀렸을때 
    /// </summary>
    [Fact]
    public async Task TryLoginAsync_Fail_InvalidCredentials()
    {
        var request = new RequestLogin
        {
            LoginId = "existingUser",
            Password = "wrongPassword" // 일부러 잘못된 비밀번호를 설정
        };

        // 사용자는 존재하지만, 비밀번호가 틀렸을 때 null을 반환하도록 설정
        _mockUserRepository.Setup(x => x.ExistUserAsync(It.IsAny<string>())).ReturnsAsync(true);
        _mockUserRepository.Setup(x => x.GetUserWithIdPasswordAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((User)null);

        // Act
        var result = await _loginService.TryLoginAsync(request);

        // Assert
        result.Data.Should().BeNull();
        result.Code.Should().Be("ERR");
        result.Message.Should().Be("아이디 혹은 비밀번호가 다릅니다.");
    }
}