using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Models.Common.Enums;
using Models.DataModels;
using Models.Requests.Login;
using Models.Responses;
using Models.Responses.Users;
using Moq;
using Providers.Repositories.Interfaces;
using Providers.Services.Implements;
using Providers.Services.Interfaces;
using Xunit;

namespace Providers.Tests.Services.Implements;

/// <summary>
/// 로그인 서비스 테스트 클래스
/// </summary>
[TestSubject(typeof(AuthenticationService))]
public class AuthenticationServiceTest
{
    // /// <summary>
    // /// 로그인 성공
    // /// </summary>
    // [Fact]
    // [Display(Description = "로그인 성공")]
    // public async Task TryLoginAsync_Success()
    // {
    //     // Arranges
    //     var mockLogger = new Mock<ILogger<AuthenticationService>>();
    //     var mockUserRepository = new Mock<IUserRepository>();
    //     var mockSignInManager = new Mock<ISignInService<DbModelUser>>();
    //     var mockUserService = new Mock<IUserService>();
    //
    //     // AuthenticationService 인스턴스 생성
    //     var authenticationService = new AuthenticationService(mockLogger.Object, mockUserRepository.Object, mockSignInManager.Object, mockUserService.Object);
    //
    //     // 요청 정보 세팅
    //     var user = new DbModelUser
    //     {
    //         LoginId = "testUser",
    //         PasswordHash = "password"
    //     };
    //
    //     // 사용자 존재 여부 및 사용자 정보 가져오기 모킹
    //     mockUserRepository.Setup(x => x.ExistUserAsync(user.LoginId)).ReturnsAsync(true);
    //     mockUserRepository.Setup(x => x.GetUserWithIdPasswordAsync(user.LoginId, user.PasswordHash))
    //         .ReturnsAsync(new DbModelUser { LoginId = user.LoginId , PasswordHash = "password"});
    //
    //     // 로그인 성공 시나리오 설정
    //     mockSignInManager.Setup(x => x.PasswordSignInAsync(user.LoginId, user.PasswordHash, false, false))
    //         .ReturnsAsync(new Response { Result = EnumResponseResult.Success });
    //
    //     // 역할 관련 모킹
    //     mockUserService.Setup(x => x.GetRolesByUserAsync())
    //         .ReturnsAsync(new ResponseList<ResponseUserRole>(EnumResponseResult.Success, "", "",
    //             new List<ResponseUserRole>()));
    //
    //     // 요청을 만든다.
    //     var request = new RequestLogin
    //     {
    //         LoginId = "testUser",
    //         Password = "password"
    //     };
    //
    //     // Act
    //     var result = await authenticationService.TryLoginAsync(request);
    //
    //     // Assert
    //     result.Should().NotBeNull();
    //     result.Result.Should().Be(EnumResponseResult.Success);
    //     result.IsAuthenticated.Should().BeTrue();
    // }
    
    // /// <summary>
    // /// 비밀번호 아이디가 틀렸을때
    // /// </summary>
    // [Fact]
    // [Display(Description = "비밀번호 아이디가 틀렸을때")]
    // public async Task TryLoginAsync_Fail_InvalidCredentials()
    // {
    //     // Arranges
    //     var mockLogger = new Mock<ILogger<AuthenticationService>>();
    //     var mockUserRepository = new Mock<IUserRepository>();
    //     var mockSignInManager = new Mock<Providers.Services.Interfaces.ISignInService<DbModelUser>>();
    //     var mockUserService = new Mock<IUserService>();
    //     var authenticationService = new AuthenticationService(mockLogger.Object, mockUserRepository.Object, mockSignInManager.Object, mockUserService.Object);
    //
    //     // 요청 정보 세팅
    //     var user = new DbModelUser
    //     {
    //         LoginId = "testUser",
    //         PasswordHash = "password"
    //     };
    //
    //     mockUserRepository.Setup(x => x.ExistUserAsync(It.IsAny<string>())).ReturnsAsync(true);
    //     mockUserRepository.Setup(x => x.GetUserWithIdPasswordAsync(user.LoginId, user.PasswordHash))
    //         .ReturnsAsync(new DbModelUser { LoginId = user.LoginId , PasswordHash = user.PasswordHash});
    //     mockSignInManager.Setup(x => x.PasswordSignInAsync(user.LoginId, user.PasswordHash, false, false))
    //         .ReturnsAsync(new Response { Message = "사용자를 찾지 못했습니다." , Result = EnumResponseResult.Error });
    //
    //     var request = new RequestLogin
    //     {
    //         LoginId = "testUser",
    //         Password = "password" // 일부러 잘못된 비밀번호를 설정
    //     };
    //
    //     // Act
    //     var result = await authenticationService.TryLoginAsync(request);
    //
    //     // Assert
    //     result.Data.Should().BeNull();
    //     result.Message.Should().Be("사용자를 찾지 못했습니다.");
    //     result.IsAuthenticated.Should().BeFalse();
    // }
}