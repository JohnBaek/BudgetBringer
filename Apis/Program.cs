using System.Reflection;
using Apis.Middlewares;
using Features.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Models.DataModels;
using Providers.Repositories;
using Providers.Repositories.Implements;
using Providers.Repositories.Interfaces;
using Providers.Services;
using Providers.Services.Implements;
using Providers.Services.Interfaces;
using Serilog;
// using Models.DataModels;

namespace Apis;

/// <summary>
/// 엔트리 프로그램
/// </summary>
public static class Program
{
    /// <summary>
    /// 메인 
    /// </summary>
    /// <param name="args">실행 변수</param>
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder.Services , builder.Configuration);

        // DB 컨텍스트 설정
        builder.Services.AddIdentity<DbModelUser, DbModelRole>()
            .AddEntityFrameworkStores<AnalysisDbContext>()
            .AddDefaultTokenProviders();

        // 스웨거 설정
        builder.Services.AddSwaggerGen(swagger =>
        {
            swagger.SwaggerDoc("v1" , new OpenApiInfo()
            {
                Version = "v1"
                , Title = "BudgetBringer API"
                , Description = ""
            });
            
            swagger.OperationFilter<IgnorePropertyFilter>();
            
            // XML 주석 파일 설정
            string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
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
    private static void ConfigureServices(IServiceCollection services, ConfigurationManager configuration)
    {
        // DB 컨텍스트 정보를 추가한다.
        string connectionString = configuration.GetConnectionString("AnalysisDatabase") ?? "";
        services.AddDbContext<AnalysisDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        // 세션 유지 설정
        services.ConfigureApplicationCookie(options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        });
        
        // 데이터 Seed 호스트 서비스 등록 ( 서비스 시작시 구동 )
        services.AddHostedService<SeedDataService>();
        // HttpContext 전역 서비스 레이어에서 사용 
        services.AddHttpContextAccessor();
        
        // 커스텀 Identity 설정 주입
        services.AddLogging();
        // 컨트롤러 설정
        services.AddControllers(config =>
        {
        });
        
        // 인증서비스 DI
        services.AddAuthentication();
        // 컨트롤러 추가
        services.AddControllers();
        
        // 빌더에 미들웨어 서비스를 추가한다.
        services.AddEndpointsApiExplorer();
        
        // 리파지토리 레이어 추가 
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IBusinessUnitRepository,BusinessUnitRepository>();
        services.AddScoped<ICostCenterRepository,CostCenterRepository>();
        services.AddScoped<ICountryBusinessManagerRepository,CountryBusinessManagerRepository>();
        services.AddScoped<ILogActionRepository,LogActionRepository>();
        services.AddScoped<ISectorRepository,SectorRepository>();
        
        // 서비스 레이어 추가
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ISignInService<DbModelUser>, SignInService>();
        services.AddScoped<IBusinessUnitService, BusinessUnitService>();
        services.AddTransient<IQueryService, QueryService>();
    }
}

