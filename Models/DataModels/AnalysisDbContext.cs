using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Models.DataModels;

/// <summary>
/// DB 컨텍스트
/// </summary>
public partial class AnalysisDbContext : IdentityDbContext<User>
{
    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="options"></param>
    public AnalysisDbContext(DbContextOptions<AnalysisDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// OnModelCreating
    /// </summary>
    /// <param name="modelBuilder">모델빌더</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Console.WriteLine(1);
        
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable(name: "User");
            entity.Property(prop => prop.LoginId)
                .HasColumnName("LoginId")
                .HasComment("로그인아이디")
                .HasMaxLength(255)
                .HasColumnType("VARCHAR")
                .IsRequired();
            entity.Property(prop => prop.LoginId)
                .HasColumnName("DisplayName")
                .HasComment("사용자명")
                .HasMaxLength(255)
                .HasColumnType("VARCHAR")
                .IsRequired();
            entity.Property(prop => prop.LastPasswordChangeDate)
                .HasColumnName("LastPasswordChangeDate")
                .HasComment("마지막 패스워드 변경일");
            // 기본 아이디 타입 변경
            // entity.Property(prop => prop.Id)
            //     .HasColumnType("CHAR")
            //     .HasMaxLength(36);
        });
        // User 테이블 키 설정 
        modelBuilder.Entity<User>()
            // 로그인 아이디 Unique
            .HasAlternateKey(prop => new { prop.LoginId });
            
        modelBuilder.Entity<IdentityRole>(entity =>
        {
            entity.ToTable(name: "Role");
        });
        modelBuilder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.ToTable("UserRoles");
            //in case you chagned the TKey type
            //  entity.HasKey(key => new { key.UserId, key.RoleId });
        });
        modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
        {
            entity.ToTable("UserClaims");
        });
        modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.ToTable("UserLogins");
            //in case you chagned the TKey type
            //  entity.HasKey(key => new { key.ProviderKey, key.LoginProvider });       
        });
        modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
        {
            entity.ToTable("RoleClaims");

        });
        modelBuilder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.ToTable("UserTokens");
        });
    }
}