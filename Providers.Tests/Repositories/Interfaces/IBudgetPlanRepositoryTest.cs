using System.Threading.Tasks;
using Features.Debounce;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Common.Enums;
using Models.DataModels;
using Models.Requests.Budgets;
using Moq;
using Providers.Repositories.Implements;
using Providers.Repositories.Interfaces;
using Providers.Services.Interfaces;
using Xunit;

namespace Providers.Tests.Repositories.Interfaces;

[TestSubject(typeof(IBudgetPlanRepository))]
public class IBudgetPlanRepositoryTest
{
    private readonly Mock<AnalysisDbContext> _mockDbContext;
    private readonly Mock<DbSet<DbModelBudgetPlan>> _mockDbSet;
    private readonly BudgetPlanRepository _repository;

    /// <summary>
    /// 생성자
    /// </summary>
    public IBudgetPlanRepositoryTest()
    {
        // _mockDbContext = new Mock<AnalysisDbContext>(new DbContextOptions<AnalysisDbContext>());
        // _mockDbSet = new Mock<DbSet<DbModelBudgetPlan>>();
        //
        // // DbContext에 DbSet을 설정
        // _mockDbContext.Setup(m => m.BudgetPlans).Returns(_mockDbSet.Object);
        //
        // // DbContext를 사용하는 Repository 인스턴스 생성
        // _repository = new BudgetPlanRepository(
        //     Mock.Of<ILogger<BudgetPlanRepository>>(),
        //     _mockDbContext.Object,
        //     Mock.Of<IQueryService>(),
        //     Mock.Of<IUserRepository>(),
        //     Mock.Of<ILogActionWriteService>(),
        //     Mock.Of<IDispatchService>(),
        // new DebounceManager(),
        //     Mock.Of<IFileService>()
        // );
    }
    
  //   /// <summary>
  //   /// 리스트에 대해서 검증한다.
  //   /// </summary>
  //   [Fact]
  //   public async Task GetListAsync_ReturnsValidData()
  //   {
  //       // Arrange
  //       var requestQuery = new RequestQuery();
  //       var expectedResponse = new ResponseList<ResponseBudgetPlan>();
  //       _mockQueryService.Setup(service => service.ToResponseListAsync(
  //               It.IsAny<RequestQuery>(),
  //               It.IsAny<Expression<Func<DbModelBudgetPlan, ResponseBudgetPlan>>>()))
  //           .ReturnsAsync(expectedResponse);
  //
  //       // Act
  //       var result = await _repository.GetListAsync(requestQuery);
  //
  //       // Assert
  //       Assert.Equal(expectedResponse, result);
  //       _mockQueryService.Verify(x => x.ToResponseListAsync(
  //   It.IsAny<RequestQuery>(),
  // It.IsAny<Expression<Func<DbModelBudgetPlan, ResponseBudgetPlan>>>()), Times.Once);
  //   }
    
    /// <summary>
    /// ADD 테스트
    /// </summary>
    [Fact]
    public async Task AddAsync_WhenInvalidRequest_ReturnsError()
    {
        // Arrange
        var request = new RequestBudgetPlan(); 
        
        // Act
        var result = await _repository.AddAsync(request);

        // Assert
        Assert.Equal(EnumResponseResult.Error, result.Result);
        Assert.Contains("필수 값을 입력해주세요", result.Message);
    }
}