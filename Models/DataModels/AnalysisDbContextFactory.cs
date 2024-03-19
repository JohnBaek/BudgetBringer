using System.Threading.Channels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Models.DataModels;

/// <summary>
/// DB 컨텍스트 팩토리 클래스 , IDesignTimeDbContextFactory 를 오버라이딩해 실행 싯점에서 핸들링된다.
/// </summary>
public class AnalysisDbContextFactory : IDesignTimeDbContextFactory<AnalysisDbContext>
{
    /// <summary>
    /// CreateDbContext 를 재정의 한다.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public AnalysisDbContext CreateDbContext(string[] args)
    {
        String basePath = Directory.GetCurrentDirectory();
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
            .Build();

        DbContextOptionsBuilder<AnalysisDbContext> builder = new DbContextOptionsBuilder<AnalysisDbContext>();
        string connectionString = configuration.GetConnectionString("AnalysisDatabase") ?? "";

        Console.WriteLine($"AnalysisDbContextFactory : {connectionString}");
        
        builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        return new AnalysisDbContext(builder.Options);
    }
}