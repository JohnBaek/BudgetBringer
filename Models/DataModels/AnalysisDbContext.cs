using Microsoft.EntityFrameworkCore;

namespace Models.DataModels;

/// <summary>
/// DB 컨텍스트
/// </summary>
public class AnalysisDbContext  : DbContext
{
    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="options"></param>
    public AnalysisDbContext(DbContextOptions<AnalysisDbContext> options) : base(options)
    {
    }
}