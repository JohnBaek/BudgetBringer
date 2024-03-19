using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Models.DataModels;

/// <summary>
/// DB 컨텍스트
/// </summary>
public partial class AnalysisDbContext : DbContext
{
    /// <summary>
    /// 역할 정보 
    /// </summary>
    public virtual DbSet<Role> Roles { get; set; }

    /// <summary>
    /// 사용자 역할 정보
    /// </summary>
    public virtual DbSet<UserRole> UserRoles { get; set; }

    /// <summary>
    /// 역할 권한 정보 
    /// </summary>
    public virtual DbSet<RoleClaim> RoleClaims { get; set; }
    
    /// <summary>
    /// 사용자 정보 
    /// </summary>
    public virtual DbSet<User> Users { get; set; }
    
    /// <summary>
    /// 사용자 로그인 정보 
    /// </summary>
    public virtual DbSet<UserLogin> UserLogins { get; set; }
    
    /// <summary>
    /// 사용자 토큰 정보 
    /// </summary>
    public virtual DbSet<UserToken> UserTokens { get; set; }
    //
    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="options"></param>
    public AnalysisDbContext(DbContextOptions<AnalysisDbContext> options) : base(options)
    {
    }
    
    /// <summary>
    /// FluentAPI 빌더 핸들링
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex").IsUnique();

            entity.Property(e => e.Id).HasComment("역할의 고유 식별자");
            entity.Property(e => e.ConcurrencyStamp).HasComment("병행 처리를 위한 스탬프");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasComment("역할 이름");
            entity.Property(e => e.NormalizedName).HasComment("역할 이름의 정규화된 형태");
        });

        modelBuilder.Entity<RoleClaim>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable(tb => tb.HasComment("역할 별 권한 정보"));

            entity.HasIndex(e => e.RoleId, "IX_RoleClaims_RoleId");

            entity.Property(e => e.Id).HasComment("역할 권한의 고유 식별자");
            entity.Property(e => e.ClaimType)
                .HasMaxLength(255)
                .HasComment("권한 유형");
            entity.Property(e => e.ClaimValue)
                .HasMaxLength(255)
                .HasComment("권한 값");
            entity.Property(e => e.RoleId).HasComment("역할의 고유 식별자");

            entity.HasOne(d => d.Role).WithMany(p => p.RoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable(tb => tb.HasComment("사용자 정보"));

            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");
            entity.HasIndex(e => e.LoginId, "LoginIdIndex");
            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex").IsUnique();
            entity.Property(e => e.Id).HasComment("아이디 값");
            entity.Property(e => e.AccessFailedCount)
                .HasComment("로그인 실패 횟수")
                .HasColumnType("int(11)");
            entity.Property(e => e.ConcurrencyStamp).HasComment("동시성 제어 스탬프");
            entity.Property(e => e.Discriminator)
                .HasMaxLength(13)
                .HasComment("유저 타입 구분자");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasComment("이메일 주소");
            entity.Property(e => e.EmailConfirmed).HasComment("이메일 인증 여부");
            entity.Property(e => e.LockoutEnabled).HasComment("계정 잠금 가능 여부");
            entity.Property(e => e.LockoutEnd)
                .HasMaxLength(6)
                .HasComment("잠금 해제 시간");
            entity.Property(e => e.LoginId).HasComment("로그인 ID");
            entity.Property(e => e.NormalizedEmail).HasComment("대문자로 변환된 이메일 주소");
            entity.Property(e => e.NormalizedUserName).HasComment("대문자로 변환된 사용자 이름");
            entity.Property(e => e.PasswordHash).HasComment("비밀번호 해시");
            entity.Property(e => e.PhoneNumber).HasComment("전화번호");
            entity.Property(e => e.PhoneNumberConfirmed).HasComment("전화번호 인증 여부");
            entity.Property(e => e.SecurityStamp).HasComment("보안 스탬프");
            entity.Property(e => e.TwoFactorEnabled).HasComment("2단계 인증 활성화 여부");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .HasComment("사용자 이름");
        });
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");

            // 기존 설정들...

            entity.Property(e => e.LoginId)
                .HasMaxLength(255)
                .HasComment("로그인 ID");

            // LoginId에 대한 유니크 인덱스 설정
            entity.HasIndex(e => e.LoginId)
                .IsUnique()
                .HasDatabaseName("IX_Users_LoginId");
            // 다른 인덱스나 설정들...
        });

        modelBuilder.Entity<UserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable(tb => tb.HasComment("사용자의 로그인 관련 정보"));

            entity.HasIndex(e => e.UserId, "IX_UserLogins_UserId");

            entity.Property(e => e.LoginProvider).HasComment("로그인 제공자");
            entity.Property(e => e.ProviderKey).HasComment("제공자 키");
            entity.Property(e => e.ProviderDisplayName)
                .HasMaxLength(255)
                .HasComment("제공자의 표시 이름");
            entity.Property(e => e.UserId).HasComment("사용자의 고유 식별자");

            entity.HasOne(d => d.User).WithMany(p => p.UserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<UserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity.ToTable(tb => tb.HasComment("사용자의 로그인 토큰 정보"));

            entity.Property(e => e.UserId).HasComment("사용자의 고유 식별자");
            entity.Property(e => e.LoginProvider).HasComment("로그인 제공자");
            entity.Property(e => e.Name).HasComment("토큰 이름");
            entity.Property(e => e.Value).HasComment("토큰 값");

            entity.HasOne(d => d.User).WithMany(p => p.UserTokens)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });
        
        modelBuilder.Entity<UserRole>(entity =>
        {
            // 복합 키 설정
            entity.HasKey(ur => new { ur.UserId, ur.RoleId }).HasName("PK_UserRoles");

            // UserId 속성 설정
            entity.Property(ur => ur.UserId)
                .IsRequired()
                .HasComment("사용자 아이디")
                .HasMaxLength(36); 

            // RoleId 속성 설정
            entity.Property(ur => ur.RoleId)
                .IsRequired()
                .HasComment("역할 아이디")
                .HasMaxLength(36); 
        });
        
        OnModelCreatingPartial(modelBuilder);
    }
    
        
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}