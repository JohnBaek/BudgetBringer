using System.Reflection;
using Apis.Middlewares;
using Features.Extensions;
using Features.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Models.DataModels;
// using Models.DataModels;
using Providers.Repositories;
using Providers.Services;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Apis;

/// <summary>
/// 엔트리 프로그램
/// </summary>
public class Program
{
    /// <summary>
    /// 메인 
    /// </summary>
    /// <param name="args">실행 변수</param>
    static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder.Services , builder.Configuration);
        
        builder.Services.AddIdentityApiEndpoints<User>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<AnalysisDbContext>();

        builder.Services.AddSwaggerGen(swagger =>
        {
            swagger.SwaggerDoc("v1" , new OpenApiInfo()
            {
                Version = "v1"
                , Title = "BudgetBrinder API"
                , Description = ""
            });
            
            swagger.OperationFilter<IgnorePropertyFilter>();
            
            // XML 주석 파일 설정
            String xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            String xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            swagger.IncludeXmlComments(xmlPath);
        });
        
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
        app.UseHandleUnauthorized();
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
        Console.WriteLine($"[CONNECTION] : {connectionString}");
        services.AddDbContext<AnalysisDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        // 커스텀 Identity 설정 주입
     
        services.AddLogging();
        services.AddControllers();
        // 인증서비스 DI
        services.AddAuthentication();
        // 컨트롤러 추가
        services.AddControllers();
        // 빌더에 미들웨어 서비스를 추가한다.
        services.AddEndpointsApiExplorer();
        
        // DI 추가
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ISignInService<User>, SignInService>();
        services.AddScoped<IUserService, UserService>();
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
            User? admin = dbContext.Users.FirstOrDefault(i => i.LoginId == "admin");
            if (admin == null)
            {
                // 관리자 계정을 생성한다.
                admin = new User()
                {
                    Id = Guid.Parse("fb5c2b27-8f0e-4789-8e6b-03ec94327be8")  ,
                    NormalizedUserName = "관리자" ,
                    DisplayName = "관리자" ,
                    LoginId = "admin" ,
                    UserName = "admin",
                    SecurityStamp = "stamps",
                    Email = "johnbaek.bjy@gmail.com" ,
                    EmailConfirmed = true ,
                    NormalizedEmail = "JOHNBAEK.BJY@GMAIL.COM" ,
                    PasswordHash = "skfdkfk1212".ToSHA() ,
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
            Role? adminRole = dbContext.Roles.FirstOrDefault(i => i.Name != null && i.Name.ToLower() == "administrator");
            
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
            
            
            // 사용자 권한 정보를 가져온다.
            Role? userRole = dbContext.Roles.FirstOrDefault(i => i.Name != null && i.Name.ToLower() == "user");
            
            // 관리자 권한 정보가 없는 경우
            if (userRole == null)
            {
                userRole = new Role()
                {
                    Id = Guid.Parse("5febd9ce-ac13-4089-b2af-cf9d51964b53"),
                    Name = "User",
                    NormalizedName = "User"
                };
            
                dbContext.Add(userRole);
                dbContext.SaveChanges();
            }
            
            // 사용자가 관리자 귄한을 쥐고 있지 않은 경우 
            if (!dbContext.UserRoles.Any(i => i.RoleId.ToString() == adminRole.Id.ToString() && i.UserId.ToString() == admin.Id.ToString()))
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

