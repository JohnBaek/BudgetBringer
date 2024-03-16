using BasicAuthenticated.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BasicAuthentication.Models;

/// <summary>
/// 컨텍스트
/// </summary>
public class BasicAuthenticatedDbContext : IdentityDbContext
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
    /// <param name="optionsBuilder">옵션빌더</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // SQLite 사용 
        optionsBuilder.UseSqlite(m_configuration.GetConnectionString("BasicAuthenticationDatabase"));
    }

    /// <summary>
    /// OnModelCreating 오버라이드
    /// </summary>
    /// <param name="builder">모델빌더</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Users 테이블로 변경 (AspNetUsers 대신)
        builder.Entity<IdentityUser>(entity =>
        {
            entity.ToTable(name: "Users");
        });

        // Roles 테이블로 변경 (AspNetRoles 대신)
        builder.Entity<IdentityRole>(entity =>
        {
            entity.ToTable(name: "Roles");
        });

        // UserClaims 테이블로 변경 (AspNetUserClaims 대신)
        builder.Entity<IdentityUserClaim<string>>(entity =>
        {
            entity.ToTable("UserClaims");
        });

        // UserLogins 테이블로 변경 (AspNetUserLogins 대신)
        builder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.ToTable("UserLogins");
        });

        // UserTokens 테이블로 변경 (AspNetUserTokens 대신)
        builder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.ToTable("UserTokens");
        });

        // RoleClaims 테이블로 변경 (AspNetRoleClaims 대신)
        builder.Entity<IdentityRoleClaim<string>>(entity =>
        {
            entity.ToTable("RoleClaims");
        });

        // UserRoles 테이블로 변경 (AspNetUserRoles 대신)
        builder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.ToTable("UserRoles");
        });
    }
}