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
    /// 로거
    /// </summary>
    private Mock<ILogger<LoginService>> _mockLogger;
    
    /// <summary>
    /// 유저 리파지토리 
    /// </summary>
    private Mock<IUserRepository> _mockUserRepository;
    
    /// <summary>
    /// 로그인서비스 
    /// </summary>
    private LoginService _loginService;

    /// <summary>
    /// 생성자
    /// </summary>
    public LoginServiceTest()
    {
        // Arranges
        _mockLogger = new Mock<ILogger<LoginService>>();
        _mockUserRepository = new Mock<IUserRepository>();
        _loginService = new LoginService(_mockLogger.Object, _mockUserRepository.Object);
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
    /// 사용자를 찾을 수 없음
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
}