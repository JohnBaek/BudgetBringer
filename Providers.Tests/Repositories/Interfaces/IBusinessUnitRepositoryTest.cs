using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

[TestSubject(typeof(IBusinessUnitRepository))]
public class BusinessUnitRepositoryTest
{
    private readonly Mock<ILogger<BusinessUnitRepository>> _mockLogger;
    private readonly Mock<IQueryService> _mockQueryService;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<ILogActionWriteService> _mockLogActionWriteService;
    private readonly Mock<AnalysisDbContext> _mockDbContext;

    public BusinessUnitRepositoryTest()
    {
        _mockLogger = new Mock<ILogger<BusinessUnitRepository>>();
        _mockQueryService = new Mock<IQueryService>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockLogActionWriteService = new Mock<ILogActionWriteService>();
        _mockDbContext = new Mock<AnalysisDbContext>(new DbContextOptions<AnalysisDbContext>());
    }

    private Mock<DbSet<T>> CreateDbSetMock<T>(IQueryable<T> data) where T : class
    {
        var dbSetMock = new Mock<DbSet<T>>();
        dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        dbSetMock.Setup(d => d.FindAsync(It.IsAny<object[]>()))
            .ReturnsAsync((object[] ids) => data.FirstOrDefault(d => ((DbModelBusinessUnit)(object)d).Id == (Guid)ids[0]));

        return dbSetMock;
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateBusinessUnit_WhenDataIsValid()
    {
        // Arrange
        var businessUnitId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var user = new DbModelUser { Id = userId, DisplayName = "TestUser" };
        var businessUnit = new DbModelBusinessUnit
        {
            Id = businessUnitId,
            Name = "OriginalName",
            RegName = "RegUser",
            ModName = "ModUser",
            RegId = user.Id,
            ModId = user.Id
        };

        var mockUserSet = CreateDbSetMock(new[] { user }.AsQueryable());
        var mockBusinessUnitSet = CreateDbSetMock(new[] { businessUnit }.AsQueryable());

        _mockDbContext.Setup(m => m.Users).Returns(mockUserSet.Object);
        _mockDbContext.Setup(m => m.BusinessUnits).Returns(mockBusinessUnitSet.Object);
        _mockDbContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        _mockUserRepository.Setup(repo => repo.GetAuthenticatedUser()).ReturnsAsync(user);

        var repository = new BusinessUnitRepository(
            _mockLogger.Object,
            _mockDbContext.Object,
            _mockQueryService.Object,
            _mockUserRepository.Object,
            _mockLogActionWriteService.Object);

        var request = new RequestBusinessUnit { Name = "UpdatedName" };

        // Act
        var response = await repository.UpdateAsync(businessUnitId.ToString(), request);

        // Assert
        Assert.Equal(EnumResponseResult.Success, response.Result);
        Assert.Equal("UpdatedName", businessUnit.Name);
        Assert.Equal(user.DisplayName, businessUnit.ModName);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnError_WhenBusinessUnitNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid().ToString();

        var mockBusinessUnitSet = CreateDbSetMock(new DbModelBusinessUnit[] { }.AsQueryable());
        _mockDbContext.Setup(m => m.BusinessUnits).Returns(mockBusinessUnitSet.Object);
        _mockDbContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var repository = new BusinessUnitRepository(
            _mockLogger.Object,
            _mockDbContext.Object,
            _mockQueryService.Object,
            _mockUserRepository.Object,
            _mockLogActionWriteService.Object);

        var request = new RequestBusinessUnit { Name = "UpdatedName" };

        // Act
        var response = await repository.UpdateAsync(nonExistentId, request);

        // Assert
        Assert.Equal(EnumResponseResult.Error, response.Result);
        Assert.Equal("ERROR_TARGET_DOES_NOT_FOUND", response.Code);
    }
}