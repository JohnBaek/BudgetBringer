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
    /// 예산 승인
    /// </summary>
    public required DbSet<BudgetApproved> BudgetApproved {get;init;}
    
    /// <summary>
    /// 예산
    /// </summary>
    public required DbSet<Budget> Budgets {get;init;}
    
    /// <summary>
    /// 비지니스 유닛
    /// </summary>
    public required DbSet<BusinessUnit> BusinessUnits {get;init;}
    
    /// <summary>
    /// 코스트센터
    /// </summary>
    public required DbSet<CostCenter> CostCenters {get;init;}
    
    /// <summary>
    /// 컨트리 비지니스 매니저
    /// </summary>
    public required DbSet<CountryBusinessManager> CountryBusinessManagers {get;init;}
    
    /// <summary>
    /// 로그액션
    /// </summary>
    public required DbSet<LogAction> LogActions {get;init;}
    
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
        modelBuilder.Entity<BudgetApproved>(entity =>{
            entity.ToTable("BudgetApproved");
        });
        modelBuilder.Entity<Budget>(entity =>{
            entity.ToTable("Budgets");
        });
        modelBuilder.Entity<BusinessUnit>(entity =>{
            entity.ToTable("BusinessUnits");
        });
        modelBuilder.Entity<CostCenter>(entity =>{
            entity.ToTable("CostCenters");
        });
        modelBuilder.Entity<CountryBusinessManager>(entity =>{
            entity.ToTable("CountryBusinessManagers");
        });
        modelBuilder.Entity<LogAction>(entity =>{
            entity.ToTable("LogActions");
        });
        modelBuilder.Entity<Role>(entity =>{
            entity.ToTable("Roles");
        });
        modelBuilder.Entity<RoleClaim>(entity =>{
            entity.ToTable("RoleClaims");
            entity.HasKey(key => new {key.Id, key.RoleId});
            entity.HasOne(ur => ur.Role)
                .WithMany(u => u.RoleClaims)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
        });
        modelBuilder.Entity<Sector>(entity =>{
            entity.ToTable("Sectors");
        });
        modelBuilder.Entity<User>(entity =>{
            entity.ToTable("Users");
        });
        modelBuilder.Entity<UserClaim>(entity =>{
            entity.ToTable("UserClaims");
            entity.HasKey(key => new {key.Id, key.UserId});
            entity.HasOne(ur => ur.User)
                .WithMany(u => u.UserClaims)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        });
        modelBuilder.Entity<UserLogin>(entity =>{
            entity.ToTable("UserLogins");
            entity.HasKey(key => new {key.LoginProvider, key.ProviderKey});
            entity.HasOne(ur => ur.User)
                .WithMany(u => u.UserLogins)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        });
        modelBuilder.Entity<UserRole>(entity =>{
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
        modelBuilder.Entity<UserToken>(entity =>{
            entity.ToTable("UserTokens");
            entity.HasKey(key => new {key.UserId, key.LoginProvider, key.Name});
            entity.HasOne(ur => ur.User)
                    .WithMany(u => u.UserTokens)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
        });
    }
}