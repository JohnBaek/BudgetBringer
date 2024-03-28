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
            
            entity.HasMany(e => e.UserClaims)
                .WithOne()
                .HasForeignKey(uc => uc.UserId)
                .IsRequired();

            // Each User can have many UserLogins
            entity.HasMany(e => e.UserLogins)
                .WithOne()
                .HasForeignKey(ul => ul.UserId)
                .IsRequired();

            // Each User can have many UserTokens
            entity.HasMany(e => e.UserTokens)
                .WithOne()
                .HasForeignKey(ut => ut.UserId)
                .IsRequired();
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
            entity.HasKey(key => new {key.UserId, key.RoleId});
            entity.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
            entity.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        });
        modelBuilder.Entity<UserClaim>(entity =>
        {
            entity.ToTable("UserClaims");
            entity.HasKey(key => new {key.Id, key.UserId});

            entity.HasOne(ur => ur.User)
                .WithMany(u => u.UserClaims)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        });
        modelBuilder.Entity<UserLogin>(entity =>
        {
            entity.ToTable("UserLogins");
            entity.HasKey(key => new {key.LoginProvider, key.ProviderKey});
            entity.HasOne(ur => ur.User)
                .WithMany(u => u.UserLogins)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        });
        modelBuilder.Entity<RoleClaim>(entity =>
        {
            entity.ToTable("RoleClaims");
            entity.HasKey(key => new {key.Id, key.RoleId});
            entity.HasOne(ur => ur.Role)
                .WithMany(u => u.RoleClaims)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
        });
        modelBuilder.Entity<UserToken>(entity =>
        {
            entity.ToTable("UserTokens");
            entity.HasKey(key => new {key.UserId, key.LoginProvider, key.Name});
            entity.HasOne(ur => ur.User)
                .WithMany(u => u.UserTokens)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        });
    }
}