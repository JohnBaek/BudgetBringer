using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models.DataModels;
using Serilog;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Apis;

/// <summary>
/// 엔트리 프로그램
/// </summary>
class Program
{
    /// <summary>
    /// 메인 
    /// </summary>
    /// <param name="args">실행 변수</param>
    static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder.Services , builder.Configuration);
        
        // 컨테이너에 서비스를 추가한다.
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        // Serilog 로거 설정
        builder.Host.UseSerilog((hostingContext, loggerConfiguration) => 
            loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));        
                
        // 웹 어플리케이션을 빌드한다.
        WebApplication app = builder.Build();

        // 디버그 환경일 경우 
        if (app.Environment.IsDevelopment())
        {
            // 스웨거 사용 
            app.UseSwagger();
            app.UseSwaggerUI();
            
            // 개발자 모드 예외처리 페이지 추가
            app.UseDeveloperExceptionPage();
        }
        // 그 외 환경
        else
        {
            // 예외시 라우팅 추가 
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }
        
        // 데이터 마이그레이션및 시드 데이터 이니셜 라이즈 
        SeedDataInitialize(app.Services);

        app.UseStaticFiles();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }

    /// <summary>
    /// 서비스 컨피그
    /// </summary>
    /// <param name="services">서비스 객체</param>
    /// <param name="configuration">컨피그 객체</param>
    public static void ConfigureServices(IServiceCollection services, ConfigurationManager configuration)
    {
        // DB 컨텍스트 정보를 추가한다.
        string connectionString = configuration.GetConnectionString("AnalysisDatabase") ?? "";
        Console.WriteLine($"connectionString : {connectionString}");
        services.AddDbContext<AnalysisDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        services.AddLogging();
        services.AddControllers();
        // 인증서비스 DI
        services.AddAuthentication();
        // 컨트롤러 추가
        services.AddControllers();
        // 빌더에 미들웨어 서비스를 추가한다.
        services.AddEndpointsApiExplorer();
    }
    
    
    /// <summary>
    /// 시드데이터를 이니셜라이즈 한다.
    /// </summary>
    /// <param name="appServices">IServiceProvider 객체</param>
    private static void SeedDataInitialize(IServiceProvider appServices)
    {
        try
        {
            using IServiceScope scope = appServices.CreateScope();
            AnalysisDbContext dbContext = scope.ServiceProvider.GetRequiredService<AnalysisDbContext>();
        
            // 데이터베이스를 이니셜라이즈한다.
            dbContext.Database.Migrate();
            
            // "관리자" 계정이 존재하지 않는 경우
            User? admin = dbContext.Users.Where(i => i.LoginId == "admin").FirstOrDefault();
            if (admin == null)
            {
                // 관리자 계정을 생성한다.
                admin = new User()
                {
                    Id = Guid.Parse("fb5c2b27-8f0e-4789-8e6b-03ec94327be8") ,
                    Discriminator = "관리자" ,
                    NormalizedUserName = "관리자" ,
                    LoginId = "admin" ,
                    Email = "johnbaek.bjy@gmail.com" ,
                    EmailConfirmed = true ,
                    NormalizedEmail = "JOHNBAEK.BJY@GMAIL.COM" ,
                    PasswordHash = "53e5ef26efa132deb014c5b96393e909ceb7fc66609e2df27c15b1be34e8f90b" ,
                    PhoneNumber = "+821033356168" ,
                    PhoneNumberConfirmed = true ,
                    TwoFactorEnabled = false ,
                    LockoutEnd = DateTime.Now ,
                    LockoutEnabled = false 
                };
                
                // 관리자 계정 등록 
                dbContext.Add(admin);
                dbContext.SaveChanges();
            }
            
            // 관리자 권한 정보를 가져온다.
            Role? adminRole = dbContext.Roles.Where(i => i.Name.ToLower() == "administrator").FirstOrDefault();
            
            // 관리자 권한 정보가 없는 경우
            if (adminRole == null)
            {
                adminRole = new Role()
                {
                    Id = Guid.Parse("6cbe5f02-dec2-44d9-a1ec-1c6590a3166b"),
                    Name = "Administrator",
                    NormalizedName = "Administrator"
                };
        
                dbContext.Add(adminRole);
                dbContext.SaveChanges();
            }
        
            // 사용자가 관리자 귄한을 쥐고 있지 않은 경우 
            if (!dbContext.UserRoles.Any(i => i.RoleId == adminRole.Id && i.UserId == admin.Id))
            {
                UserRole addUserRole = new UserRole()
                {
                    UserId = admin.Id ,
                    RoleId = adminRole.Id
                };
                // 사용자 궎한을 추가한다.
                dbContext.Add(addUserRole);
                dbContext.SaveChanges();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}