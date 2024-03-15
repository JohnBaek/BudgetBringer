using BasicAuthenticated.Resources;
using Microsoft.EntityFrameworkCore;

namespace BasicAuthentication.Models;

/// <summary>
/// 컨텍스트
/// </summary>
public class BasicAuthenticatedDbContext : DbContext
{
    /// <summary>
    /// 컨피그
    /// </summary>
    public IConfiguration m_configuration;

    /// <summary>
    /// 유저 정보 
    /// </summary>
    public DbSet<User> Users { get; set; }
    
    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="configuration"></param>
    public BasicAuthenticatedDbContext(IConfiguration configuration)
    {
        m_configuration = configuration;
    }
    
    /// <summary>
    /// OnConfiguring 오버라이드
    /// </summary>
    /// <param name="optionsBuilder"></param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // SQLite 사용 
        optionsBuilder.UseSqlite(m_configuration.GetConnectionString("BasicAuthenticationDatabase"));
    }
}