using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Models.Common.Enums;
using Models.Requests.Budgets;
using Models.Requests.Query;
using Models.Responses;
using Models.Responses.Budgets;
using Moq;
using Providers.Repositories.Interfaces;
using Providers.Services.Implements;
using Xunit;

namespace Providers.Tests.Services.Implements;

/// <summary>
/// 예산 계획 서비스 테스트
/// </summary>
[TestSubject(typeof(BudgetPlanService))]
public class BudgetPlanServiceTest
{
    private readonly Mock<IBudgetPlanRepository> _mockRepository;
    private readonly BudgetPlanService _service;
    
    public BudgetPlanServiceTest()
    {
        var mockLogger = new Mock<ILogger<BudgetPlanService>>();
        _mockRepository = new Mock<IBudgetPlanRepository>();
        _service = new BudgetPlanService(_mockRepository.Object, mockLogger.Object);
    }
    
    /// <summary>
    /// 리스트를 가져온다.
    /// </summary>
    [Fact]
    [Display(Description = "올바른 리스트를 가져온다.")]
    public async Task GetListAsync_ReturnsValidData()
    {
        // Arrange
        var requestQuery = new RequestQuery();
        var expectedResponse = new ResponseList<ResponseBudgetPlan>();
        _mockRepository.Setup(repo => repo.GetListAsync(requestQuery))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _service.GetListAsync(requestQuery);

        // Assert
        expectedResponse.Should().Be(result);
        _mockRepository.Verify(repo => repo.GetListAsync(requestQuery), Times.Once);
    }
    
    /// <summary>
    /// 데이터를 검증한다.
    /// </summary>
    [Fact]
    public async Task GetAsync_ReturnsData()
    {
        // Arrange
        string id = "testId";
        var expectedResponse = new ResponseData<ResponseBudgetPlan>();
        _mockRepository.Setup(repo => repo.GetAsync(id))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _service.GetAsync(id);

        // Assert
        expectedResponse.Should().Be(result);
        _mockRepository.Verify(repo => repo.GetAsync(id), Times.Once);
    }
    
    
    /// <summary>
    /// 업데이트를 검증한다.
    /// </summary>
    [Fact]
    public async Task UpdateAsync_ReturnsSuccessResponse()
    {
        // Arrange
        string id = "123";
        var request = new RequestBudgetPlan();
        var expectedResponse = new Response { Result = EnumResponseResult.Success };
        _mockRepository.Setup(r => r.UpdateAsync(id, request)).ReturnsAsync(expectedResponse);

        // Act
        var result = await _service.UpdateAsync(id, request);

        // Assert
        Assert.Equal(expectedResponse, result);
        _mockRepository.Verify(r => r.UpdateAsync(id, request), Times.Once);
    }

    /// <summary>
    /// 데이터 추가를 검증한다.
    /// </summary>
    [Fact]
    public async Task AddAsync_ReturnsSuccessResponse()
    {
        // Arrange
        var request = new RequestBudgetPlan();
        var expectedResponse = new ResponseData<ResponseBudgetPlan> { Result = EnumResponseResult.Success };
        _mockRepository.Setup(r => r.AddAsync(request)).ReturnsAsync(expectedResponse);

        // Act
        var result = await _service.AddAsync(request);

        // Assert
        Assert.Equal(expectedResponse, result);
        _mockRepository.Verify(r => r.AddAsync(request), Times.Once);
    }

    /// <summary>
    /// 데이터 삭제를 검증한다.
    /// </summary>
    [Fact]
    public async Task DeleteAsync_ReturnsSuccessResponse()
    {
        // Arrange
        string id = "123";
        var expectedResponse = new Response { Result = EnumResponseResult.Success };
        _mockRepository.Setup(r => r.DeleteAsync(id)).ReturnsAsync(expectedResponse);

        // Act
        var result = await _service.DeleteAsync(id);

        // Assert
        Assert.Equal(expectedResponse, result);
        _mockRepository.Verify(r => r.DeleteAsync(id), Times.Once);
    }
}