using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Models.Common.Enums;
using Models.DataModels;
using Models.Requests.Login;
using Models.Responses;
using Moq;
using Providers.Repositories;
using Providers.Repositories.Interfaces;
using Providers.Services;
using Providers.Services.Implements;
using Providers.Services.Interfaces;
using Xunit;

namespace Providers.Tests.Services;

/// <summary>
/// 로그인 서비스 테스트 클래스
/// </summary>
[TestSubject(typeof(AuthenticationService))]
public class AuthenticationServiceTest
{
    /// <summary>
    /// 유저 리파지토리 
    /// </summary>
    private Mock<IUserRepository> _mockUserRepository;
    
    /// <summary>
    /// 로그인서비스 
    /// </summary>
    private AuthenticationService _authenticationService;
    
    /// <summary>
    /// 로그인 성공
    /// </summary>
    [Fact]
    public async Task TryLoginAsync_Success()
    {
        // Arranges
        var mockLogger = new Mock<ILogger<AuthenticationService>>();
        var mockUserRepository = new Mock<IUserRepository>();
        var mockSignInManager = new Mock<ISignInService<DbModelUser>>();
    
        // AuthenticationService 인스턴스 생성
        var authenticationService = new AuthenticationService(mockLogger.Object, mockUserRepository.Object, mockSignInManager.Object);
    
        // 요청 정보 세팅
        var user = new DbModelUser
        {
            LoginId = "testUser",
            PasswordHash = "password"
        };

        // 사용자 존재 여부 및 사용자 정보 가져오기 모킹
        mockUserRepository.Setup(x => x.ExistUserAsync(user.LoginId)).ReturnsAsync(true);
        mockUserRepository.Setup(x => x.GetUserWithIdPasswordAsync(user.LoginId, user.PasswordHash))
            .ReturnsAsync(new DbModelUser { LoginId = user.LoginId , PasswordHash = "password"});
        
        // 로그인 성공 시나리오 설정
        mockSignInManager.Setup(x => x.PasswordSignInAsync(user.LoginId, user.PasswordHash, false, false))
            .ReturnsAsync(new Response { Result = EnumResponseResult.Success });

        var request = new RequestLogin
        {
            LoginId = "testUser",
            Password = "password"
        };
        
        // Act
        var result = await authenticationService.TryLoginAsync(request);
    
        // Assert
        result.Should().NotBeNull();
        result.Result.Should().Be(EnumResponseResult.Success);
        result.IsAuthenticated.Should().BeTrue();
    }
    
    /// <summary>
    /// 사용자를 찾을수 없을때
    /// </summary>
    [Fact]
    public async Task TryLoginAsync_Fail_UserNotFound()
    {
        // Arranges
        var mockLogger = new Mock<ILogger<AuthenticationService>>();
        var mockSignInManager = new Mock<ISignInService<DbModelUser>>();
        _mockUserRepository = new Mock<IUserRepository>();
        _authenticationService = new AuthenticationService(mockLogger.Object, _mockUserRepository.Object , mockSignInManager.Object);
        
        var request = new RequestLogin
        {
            LoginId = "nonExistingUser",
            Password = "password"
        };
    
        _mockUserRepository.Setup(x => x.ExistUserAsync(It.IsAny<string>())).ReturnsAsync(false);
    
        // Act
        var result = await _authenticationService.TryLoginAsync(request);
    
        // Assert
        result.Data.Should().BeNull();
        result.Code.Should().Be("ERR");
        result.Message.Should().Be("사용자를 찾지 못했습니다.");
    }
    
    /// <summary>
    /// 비밀번호 아이디가 틀렸을때 
    /// </summary>
    [Fact]
    [Display(Description = "비밀번호 아이디가 틀렸을때 검증")]
    public async Task TryLoginAsync_Fail_InvalidCredentials()
    {
        // Arranges
        var mockLogger = new Mock<ILogger<AuthenticationService>>();
        var mockSignInManager = new Mock<ISignInService<DbModelUser>>();
        _mockUserRepository = new Mock<IUserRepository>();
        _authenticationService = new AuthenticationService(mockLogger.Object, _mockUserRepository.Object , mockSignInManager.Object);
        
        var request = new RequestLogin
        {
            LoginId = "existingUser",
            Password = "wrongPassword" // 일부러 잘못된 비밀번호를 설정
        };
    
        // 사용자는 존재하지만, 비밀번호가 틀렸을 때 null을 반환하도록 설정
        _mockUserRepository.Setup(x => x.ExistUserAsync(It.IsAny<string>())).ReturnsAsync(true);
        _mockUserRepository.Setup(x => x.GetUserWithIdPasswordAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((DbModelUser)null);
    
        // Act
        var result = await _authenticationService.TryLoginAsync(request);
    
        // Assert
        result.Data.Should().BeNull();
        result.Code.Should().Be("ERR");
        result.Message.Should().Be("아이디 혹은 비밀번호가 다릅니다.");
    }
}