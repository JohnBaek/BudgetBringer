using System.Security.Claims;
using Features.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models.DataModels;

namespace Providers.Services.Implements;

/// <summary>
/// Seed 데이터 서비스 구현체
/// </summary>
public class SeedDataService : IHostedService
{
    /// <summary>
    /// 서비스 프로바이더 IHostedService 에서 주입
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="serviceProvider">서비스 프로바이더</param>
    public SeedDataService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    
    /// <summary>
    /// Sync 를 시작한다.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // 서비스 포인트를 만든다.
        using IServiceScope scope = _serviceProvider.CreateScope();
        
        // 데이터 베이스 컨텍스트를 가져온다.
        AnalysisDbContext dbContext = scope.ServiceProvider.GetRequiredService<AnalysisDbContext>();
        
        Console.WriteLine("[Migration Start]".WithDateTime());    
        // 마이그레이션을 시작한다.
        await dbContext.Database.MigrateAsync(cancellationToken);
        Console.WriteLine("[Migration End]".WithDateTime());    
        Console.WriteLine("[DbModelUser and DbModelRole Initialize Start]".WithDateTime());

        // // 필요한 매니저 서비스를 DI 한다.
        UserManager<DbModelUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<DbModelUser>>();
        RoleManager<DbModelRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<DbModelRole>>();
        //
        // // 관리자 , 및 유저에 대한 권한을 생성한다.
        // await CreateRoleAsync(roleManager,"Admin");
        // await CreateRoleAsync(roleManager,"User");
        //
        // // 상세 관리자 Claim 을 설정한다.
        // await CreateRoleClaimAdminAsync(dbContext,roleManager);
        // // 상세 유저 Claim 을 설정한다.
        // await CreateRoleClaimUserAsync(dbContext,roleManager);
        //
        // // 관리자를 생성한다.
        // await CreateUserAsync(userManager: userManager , role:"Admin" , loginId:"admin" , displayName:"관리자", password:"Qkfka!@12" );
        // await CreateUserAsync(userManager: userManager , role:"Admin" , loginId:"sgs_mike_admin" , displayName:"Mike(Admin)", password:"Pass%word12" );
        // // 사용자를 생성한다.
        // await CreateUserAsync(userManager: userManager , role:"User" , loginId:"commonuser" , displayName:"일반 사용자", password:"Qkfka!@12" );
        // await CreateUserAsync(userManager: userManager , role:"User" , loginId:"sgs_AnoldKim" , displayName:"Anold Kim", password:"Pass%word1" );
        // await CreateUserAsync(userManager: userManager , role:"User" , loginId:"sgs_ByungchanHong" , displayName:"Byungchan Hong", password:"Pass%word2" );
        // await CreateUserAsync(userManager: userManager , role:"User" , loginId:"sgs_JeffJang" , displayName:"Jeff Jang", password:"Pass%word3" );
        // await CreateUserAsync(userManager: userManager , role:"User" , loginId:"sgs_DerekLee" , displayName:"Derek Lee", password:"Pass%word4" );
        // await CreateUserAsync(userManager: userManager , role:"User" , loginId:"sgs_AlbertLim" , displayName:"Albert Lim", password:"Pass%word5" );
        // await CreateUserAsync(userManager: userManager , role:"User" , loginId:"sgs_YuriHong" , displayName:"Yuri Hong", password:"Pass%word6" );
        // await CreateUserAsync(userManager: userManager , role:"User" , loginId:"sgs_WuhongJu" , displayName:"Wuhong Ju", password:"Pass%word7" );
        // await CreateUserAsync(userManager: userManager , role:"User" , loginId:"sgs_BCHong" , displayName:"BC Hong", password:"Pass%word8" );
        // await CreateUserAsync(userManager: userManager , role:"User" , loginId:"sgs_SandersBae" , displayName:"Sanders Bae", password:"Pass%word9" );
        // await CreateUserAsync(userManager: userManager , role:"User" , loginId:"sgs_BruceMoon" , displayName:"Bruce Moon", password:"Pass%word10" );
        //
        // await CreateUserWithSpecifyAsync(userManager , role:"User" , loginId:"sgs" , displayName:"SGS", password:"Qkfka!212", targetPermission: "process-result" , description: "결과페이지 전체 권한 부여" );
        // await CreateUserWithSpecifyAsync(userManager , role:"User" , loginId:"sgs_mike_user" , displayName:"Mike", password:"Pass%word12", targetPermission: "process-result" , description: "결과페이지 전체 권한 부여" );
        
        await CreateUserAsync(userManager: userManager , role:"Admin" , loginId:"capexadmin1" , displayName:"Capex관리자1", password:"Qkfka!@34" );
        await CreateUserAsync(userManager: userManager , role:"Admin" , loginId:"capexadmin2" , displayName:"Capex관리자2", password:"Qkfka!@34" );
        
        Console.WriteLine("[User and DbModelRole Initialize End]".WithDateTime());    
    }


    /// <summary>
    /// 관리자의 상세 Claim 을 설정한다.
    /// </summary>
    /// <param name="dbContext">데이터베이스 컨텍스트</param>
    /// <param name="roleManager">역할 매니저</param>
    private async Task CreateRoleClaimAdminAsync(AnalysisDbContext dbContext, RoleManager<DbModelRole> roleManager)
    {
        try
        {
            DbModelRole? role = await roleManager.FindByNameAsync("Admin");
            if (role == null)
            {
                Console.WriteLine("Admin 역할이 존재하지 않습니다.");
                return;
            }
            
            // 상세 역할 지정
            var requiredClaims = new List<DbModelRoleClaim>
            {
                // 공통 코드 관리
                new() { DbModelRole = role,  Id = 11000 , ClaimType = "Permission", ClaimValue = "common-code", DisplayName = "공통코드", Description ="공통코드 전체권한"},
                
                // 예산 입력
                new() { DbModelRole = role,  Id = 12000 , ClaimType = "Permission", ClaimValue = "budget-plan" , DisplayName = "예산계획" , Description = "예산계획 전체권한"},
                
                // 예산 승인
                new() { DbModelRole = role,  Id = 13000 , ClaimType = "Permission", ClaimValue = "budget-approved" , DisplayName = "예산승인", Description = "예산승인 전체권한"},
                
                // 액션 로그
                new() { DbModelRole = role,  Id = 14000 , ClaimType = "Permission", ClaimValue = "log-action", DisplayName = "액션 로그", Description = "액션 로그 전체권한"},
                new() { DbModelRole = role,  Id = 14001 , ClaimType = "Permission", ClaimValue = "log-action-view", DisplayName = "액션 로그", Description = "액션 로그 View 권한"},
                
                // 결과 확인
                new() { DbModelRole = role,  Id = 15000 , ClaimType = "Permission", ClaimValue = "process-result", DisplayName = "결과 확인", Description = "결과 확인 전체권한"},
                new() { DbModelRole = role,  Id = 15001 , ClaimType = "Permission", ClaimValue = "process-view", DisplayName = "결과 확인", Description = "결과 View 권한"},
            };
            
            // 모든 Claim 에 대해 처리한다.
            int count = 0;
            foreach (var requiredClaim in requiredClaims)
            {
                // 요청하는 Claim이 없는지 여부 
                bool hasNotClaim = !dbContext.RoleClaims.Any(i =>
                    i.RoleId == role.Id &&
                    i.ClaimType == requiredClaim.ClaimType && 
                    i.ClaimValue == requiredClaim.ClaimValue);
                
                // 현재 Claim 에 필요한 값이 없는경우 
                if (hasNotClaim)
                {
                    await dbContext.RoleClaims.AddAsync(requiredClaim);
                    Console.WriteLine($"[{++count}][관리자 Claim 추가] : ClaimType : [{requiredClaim.ClaimType}] , ClaimValue : [{requiredClaim.ClaimValue}]".WithDateTime());
                    await dbContext.SaveChangesAsync();
                }
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /// <summary>
    /// 사용자의 상세 Claim 을 설정한다.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="roleManager"></param>
    private async Task CreateRoleClaimUserAsync(AnalysisDbContext dbContext, RoleManager<DbModelRole> roleManager)
    {
        try
        {
            DbModelRole? role = await roleManager.FindByNameAsync("User");
            if (role == null)
            {
                Console.WriteLine("User 역할이 존재하지 않습니다.");
                return;
            }
            
            // 상세 역할 지정
            var requiredClaims = new List<DbModelRoleClaim>
            {
                // 결과 확인
                new() { DbModelRole = role,  Id = 15001 , ClaimType = "Permission", ClaimValue = "process-result-view", DisplayName = "결과 확인", Description = "결과 확인"},
            };
            
            // 모든 Claim 에 대해 처리한다.
            int count = 0;
            foreach (var requiredClaim in requiredClaims)
            {
                // 요청하는 Claim이 없는지 여부 
                bool hasNotClaim = !dbContext.RoleClaims.Any(i =>
                    i.RoleId == role.Id &&
                    i.ClaimType == requiredClaim.ClaimType && 
                    i.ClaimValue == requiredClaim.ClaimValue);
                
                // 현재 Claim 에 필요한 값이 없는경우 
                if (hasNotClaim)
                {
                    await dbContext.RoleClaims.AddRangeAsync(requiredClaim);
                    Console.WriteLine($"[{++count}][사용자 Claim 추가] : ClaimType - [{requiredClaim.ClaimType}] , ClaimValue - [{requiredClaim.ClaimValue}]".WithDateTime());
                }
            }

            if(count > 0)
                await dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /// <summary>
    /// Task 를 정지한다.
    /// </summary>
    /// <param name="cancellationToken">취소 토큰정보</param>
    /// <returns></returns>
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    /// <summary>
    /// 사용자 정보를 생성한다.
    /// </summary>
    /// <param name="userManager">사용자 매니저</param>
    /// <param name="role">역할 정보</param>
    /// <param name="loginId">로그인아이디</param>
    /// <param name="displayName">사용자명</param>
    /// <param name="password">패스워드</param>
    private async Task CreateUserAsync(UserManager<DbModelUser> userManager,  string role,string loginId, string displayName ,string password)
    {
        // 생성될 유저 정보
        DbModelUser adminDbModelUser = new DbModelUser{ 
            LoginId = loginId
            , Id = Guid.NewGuid()
            , UserName = loginId
            , Email = $"{loginId}@sgs.com"
            , DisplayName = displayName
            , PasswordHash = password
        };
            
        // 사용자가 존재하지 않는경우 
        if (await userManager.FindByNameAsync(adminDbModelUser.UserName) == null)
        {
            IdentityResult result = await userManager.CreateAsync(adminDbModelUser, password);
            Console.WriteLine($"[사용자 추가] : 사용자명 - [{adminDbModelUser.DisplayName}]".WithDateTime());
            
            if (!result.Succeeded)
            {
                // 실패한 경우, 에러 메시지 로깅 또는 처리
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Error: {error.Description}".WithDateTime());
                }
            }
            
            await userManager.AddToRoleAsync(adminDbModelUser, role.ToUpper());
        }
    }


    /// <summary>
    /// 특수 사용자 생성
    /// </summary>
    /// <param name="userManager">사용자 매니저</param>
    /// <param name="role">역할 정보</param>
    /// <param name="loginId">로그인아이디</param>
    /// <param name="displayName">사용자명</param>
    /// <param name="password">패스워드</param>
    /// <param name="targetPermission">대상 권한</param>
    /// <param name="description">설명</param>
    private async Task CreateUserWithSpecifyAsync(UserManager<DbModelUser> userManager, string role, string loginId,
        string displayName, string password, string targetPermission, string description)
    {
        // 기본 사용자를 생성 
        await CreateUserAsync(userManager: userManager, role:role, loginId: loginId, displayName: displayName, password: password);

        // 생성된 사용자 정보를 검색
        DbModelUser? findUser =  await userManager.FindByNameAsync(loginId);
        
        // 찾은 정보가 없는 경우 
        if (findUser == null)
            return;
        
        // 부여할 클레임
        Claim requiredClaim = new Claim("Permission", targetPermission);
        
        // 현재 사용자의 클레임 정보
        IList<Claim> currentClaims = await userManager.GetClaimsAsync(findUser);

        // 현재 사용자에게 필요한 클레임이 없는 경우 부여
        if (!currentClaims.Any(claim => claim.Type == requiredClaim.Type && claim.Value == requiredClaim.Value))
        {
            await userManager.AddClaimAsync(findUser, requiredClaim);
            
        }
    }


    /// <summary>
    /// 역할을 추가한다.
    /// </summary>
    /// <param name="roleManager"></param>
    /// <param name="roleName">추가할 역할</param>
    private async Task CreateRoleAsync( RoleManager<DbModelRole> roleManager , string roleName)
    {
        // 대상 역할이 없는경우 
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            DbModelRole add = new DbModelRole
            {
                Name = roleName,
                Id = Guid.NewGuid()
            };
            Console.WriteLine($"[역할정보 추가] : 역할명 - [{add.Name}]".WithDateTime());
            await roleManager.CreateAsync(add);
        }

    }
} 