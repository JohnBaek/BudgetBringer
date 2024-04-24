using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Models.DataModels;

/// <summary>
/// DB 컨텍스트
/// </summary>
public partial class AnalysisDbContext : IdentityDbContext<DbModelUser, DbModelRole, Guid , DbModelUserClaim , DbModelUserRole , DbModelUserLogin , DbModelRoleClaim, DbModelUserToken>
{
    /// <summary>
    /// 예산 승인
    /// </summary>
    public required DbSet<DbModelBudgetApproved> BudgetApproved {get;init;}
    
    /// <summary>
    /// 예산
    /// </summary>
    public required DbSet<DbModelBudgetPlan> BudgetPlans {get;init;}
    
    /// <summary>
    /// 비지니스 유닛
    /// </summary>
    public required DbSet<DbModelBusinessUnit> BusinessUnits {get;init;}
    
    /// <summary>
    /// 섹터
    /// </summary>
    public required DbSet<DbModelSector> Sectors {get;init;}
    
    /// <summary>
    /// 코스트센터
    /// </summary>
    public required DbSet<DbModelCostCenter> CostCenters {get;init;}
    
    /// <summary>
    /// 컨트리 비지니스 매니저
    /// </summary>
    public required DbSet<DbModelCountryBusinessManager> CountryBusinessManagers {get;init;}
    
    /// <summary>
    /// 로그액션
    /// </summary>
    public required DbSet<DbModelLogAction> LogActions {get;init;}
    
    /// <summary>
    /// 컨트리 비지니스 매니저(1) : 비지니스 유닛 (N) 관계 테이블 
    /// </summary>
    public required DbSet<DbModelCountryBusinessManagerBusinessUnit> CountryBusinessManagerBusinessUnits { get; set; }

    /// <summary>
    /// 통계 캐시 테이블
    /// </summary>
    public required DbSet<DbModelBudgetAnalysisCache>  BudgetAnalysisCache { get; set; }
    
    /// <summary>
    /// System Common Config 
    /// </summary>
    public required DbSet<DbModelSystemConfig> SystemConfigs { get; set; }
    
    /// <summary>
    /// Detail of Configs
    /// </summary>
    public required DbSet<DbModelSystemConfigDetail> SystemConfigDetail { get; set; }
    
    
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
        modelBuilder.Entity<DbModelBudgetApproved>(entity =>{
            entity.ToTable("BudgetApproved");
        });
        modelBuilder.Entity<DbModelBudgetPlan>(entity =>{
            entity.ToTable("BudgetPlans");
        });
        modelBuilder.Entity<DbModelBusinessUnit>(entity =>{
            entity.ToTable("BusinessUnits");
        });
        modelBuilder.Entity<DbModelCostCenter>(entity =>{
            entity.ToTable("CostCenters");
        });
        modelBuilder.Entity<DbModelCountryBusinessManager>(entity =>{
            entity.ToTable("CountryBusinessManagers");
        });
        modelBuilder.Entity<DbModelLogAction>(entity =>{
            entity.ToTable("LogActions");
            entity.Property(i => i.Contents)
                .HasColumnType("TEXT");
        });
        modelBuilder.Entity<DbModelRole>(entity =>{
            entity.ToTable("Roles");
        });
        modelBuilder.Entity<DbModelRoleClaim>(entity =>{
            entity.ToTable("RoleClaims");
            entity.HasKey(key => new {key.Id, key.RoleId});
            entity.HasOne(ur => ur.DbModelRole)
                .WithMany(u => u.RoleClaims)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
        });
        modelBuilder.Entity<DbModelSector>(entity =>{
            entity.ToTable("Sectors");
        });
        modelBuilder.Entity<DbModelUser>(entity =>{
            entity.ToTable("Users");
        });
        modelBuilder.Entity<DbModelUserClaim>(entity =>{
            entity.ToTable("UserClaims");
            entity.HasKey(key => new {key.Id, key.UserId});
            entity.HasOne(ur => ur.DbModelUser)
                .WithMany(u => u.UserClaims)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        });
        modelBuilder.Entity<DbModelUserLogin>(entity =>{
            entity.ToTable("UserLogins");
            entity.HasKey(key => new {key.LoginProvider, key.ProviderKey});
            entity.HasOne(ur => ur.DbModelUser)
                .WithMany(u => u.UserLogins)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        });
        modelBuilder.Entity<DbModelUserRole>(entity =>{
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
        modelBuilder.Entity<DbModelUserToken>(entity =>{
            entity.ToTable("UserTokens");
            entity.HasKey(key => new {key.UserId, key.LoginProvider, key.Name});
            entity.HasOne(ur => ur.DbModelUser)
                    .WithMany(u => u.UserTokens)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
        });
        modelBuilder.Entity<DbModelCountryBusinessManagerBusinessUnit>(entity =>
        {
            entity.ToTable("CountryBusinessManagerBusinessUnit");
            entity.HasKey(bc => new { bc.CountryBusinessManagerId, bc.BusinessUnitId });
            entity.HasOne(bc => bc.CountryBusinessManager)
                .WithMany(b => b.CountryBusinessManagerBusinessUnits)
                .HasForeignKey(bc => bc.CountryBusinessManagerId);
            entity.HasOne(bc => bc.BusinessUnit)
                .WithMany(c => c.CountryBusinessManagers)
                .HasForeignKey(bc => bc.BusinessUnitId);
        });
        modelBuilder.Entity<DbModelBudgetAnalysisCache>(entity =>
        {
            entity.ToTable("BudgetAnalysisCache");
            entity.HasKey(i => i.Id);
            entity.Property(i => i.JsonRaw)
                .HasColumnType("LONGTEXT");
        });

        modelBuilder.Entity<DbModelSystemConfig>();
        modelBuilder.Entity<DbModelSystemConfigDetail>();
    }
}