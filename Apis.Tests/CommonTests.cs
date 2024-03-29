using System.Net;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Models.Common.Enums;
using Models.Responses;
using Moq;
using Moq.Protected;
using Xunit;

namespace Apis.Tests;

/// <summary>
/// 공통요소에 대한 테스트 
/// </summary>
public class CommonTests
{
    /// <summary>
    /// 로그인 되지 않은 사용자의 경우
    /// 메세지는 따로 핸들링되어 사용자에게 표기되어야 한다.
    /// </summary>
    [Fact]
    public async Task Request_ChangeResponseMessage_WhenNotAuthorized()
    {
        // HttpMessageHandler Mock 생성
        var handlerMock = new Mock<HttpMessageHandler>();

        // Mock HttpMessageHandler를 사용하여 예상되는 응답 설정
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Content = new StringContent("{\"Code\":\"ATH099\",\"Message\":\"로그인 후 이용해주세요\",\"IsAuthenticated\":false,\"Result\":-99}")
            })
            .Verifiable();

        // HttpClient 생성 및 Mock Handler 주입
        var httpClient = new HttpClient(handlerMock.Object);

        // Act
        var response = await httpClient.GetAsync("http://localhost/api/v1/user");
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<Response>(content);

        // Assert
        HttpStatusCode.Unauthorized.Should().Be(response.StatusCode);
        
        result?.Message.Should().NotBeEmpty("메세지는 비어있으면 안됩니다.");
        result?.Message.Should().Be("로그인 후 이용해주세요");
        result?.Result.Should().Be(EnumResponseResult.Error);

        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
        );
    }
    
    /// <summary>
    /// 로그인은 되었지만 권한이 없는 유저인경우
    /// 메세지는 따로 핸들링되어 사용자에게 표기되어야 한다.
    /// </summary>
    [Fact]
    public async Task Request_ChangeResponseMessage_WhenNotHavePermissions()
    {
        // HttpMessageHandler Mock 생성
        var handlerMock = new Mock<HttpMessageHandler>();

        // Mock HttpMessageHandler를 사용하여 예상되는 응답 설정
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Forbidden,
                Content = new StringContent("{\"Code\":\"ATH099\",\"Message\":\"접근 권한이 없습니다.\",\"IsAuthenticated\":false,\"Result\":-99}")
            })
            .Verifiable();

        // HttpClient 생성 및 Mock Handler 주입
        var httpClient = new HttpClient(handlerMock.Object);

        // Act
        var response = await httpClient.GetAsync("http://localhost/api/v1/user");
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<Response>(content);

        // Assert
        HttpStatusCode.Forbidden.Should().Be(response.StatusCode);
        
        result?.Message.Should().NotBeEmpty("메세지는 비어있으면 안됩니다.");
        result?.Message.Should().Be("접근 권한이 없습니다.");
        result?.Result.Should().Be(EnumResponseResult.Error);

        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
        );
    }
}
