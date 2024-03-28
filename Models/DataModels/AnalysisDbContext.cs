using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Models.DataModels;

/// <summary>
/// DB 컨텍스트
/// </summary>
public partial class AnalysisDbContext : IdentityDbContext<User, Role, Guid , UserClaim , UserRole , UserLogin , RoleClaim, UserToken>
{
    /**
     * 사용자 정보
     */
    private DbSet<User> Users;
    
    /**
     * 역할 정보
     */
    private DbSet<Role> Roles;
    
    private DbSet<UserClaim> UserClaims;
    private DbSet<UserRole> UserRoles;
    private DbSet<UserLogin> UserLogins;
    private DbSet<RoleClaim> RoleClaims;
    private DbSet<UserToken> UserTokens;
    
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
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable(name: "Users");
            entity.Property(prop => prop.LoginId)
                .HasColumnName("LoginId")
                .HasComment("로그인아이디")
                .HasMaxLength(255)
                .HasColumnType("VARCHAR")
                .IsRequired();
            entity.Property(prop => prop.DisplayName)
                .HasColumnName("DisplayName")
                .HasComment("사용자명")
                .HasMaxLength(255)
                .HasColumnType("VARCHAR")
                .IsRequired();
            entity.Property(prop => prop.LastPasswordChangeDate)
                .HasColumnName("LastPasswordChangeDate")
                .HasComment("마지막 패스워드 변경일");
        });
        // User 테이블 키 설정 
        modelBuilder.Entity<User>()
            // 로그인 아이디 Unique
            .HasAlternateKey(prop => new { prop.LoginId });
            
        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable(name: "Roles");
        });
        
        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable("UserRoles");
            entity.HasKey(key => new { key.UserId, key.RoleId });
        });
        modelBuilder.Entity<UserClaim>(entity =>
        {
            entity.ToTable("UserClaims");
        });
        modelBuilder.Entity<UserLogin>(entity =>
        {
            entity.ToTable("UserLogins");
            entity.HasKey(key => new { key.ProviderKey, key.LoginProvider });       
        });
        modelBuilder.Entity<RoleClaim>(entity =>
        {
            entity.ToTable("RoleClaims");

        });
        modelBuilder.Entity<UserToken>(entity =>
        {
            entity.ToTable("UserTokens");
            entity.HasKey(key => new { key.UserId, key.LoginProvider });       
        });
    }
}