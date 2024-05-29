using System.Reflection;
using Apis.Middlewares;
using Features.Debounce;
using Features.Filters;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Models.DataModels;
using Providers.Repositories.Implements;
using Providers.Repositories.Interfaces;
using Providers.Services.Implements;
using Providers.Services.Interfaces;
using Serilog;

namespace Apis;

/// <summary>
/// Root Program
/// </summary>
public static class Program
{
    /// <summary>
    /// Main 
    /// </summary>
    /// <param name="args">실행 변수</param>
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        
        ConfigureServices(builder.Services , builder.Configuration , builder.Environment);

        // DB 컨텍스트 설정
        builder.Services.AddIdentity<DbModelUser, DbModelRole>()
            .AddEntityFrameworkStores<AnalysisDbContext>()
            .AddDefaultTokenProviders();

        // Reverse Proxy 설정
        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });

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
            app.UseSwagger();
            // 예외시 라우팅 추가 
            app.UseExceptionHandler("/Error");
            app.UseForwardedHeaders();
            // app.UseHsts();
        }

        // File RealPath
        string staticFileDirectory = app.Environment.IsDevelopment() ? 
            "/Users/john/Library/Caches/Budget/Persist" :
            "/data-files/file-persist";
        
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(staticFileDirectory),
            // URL root path , should configured in nginx
            RequestPath =  "/files"  
        });
        
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
    /// <param name="builderEnvironment">호스팅 환경 객체</param>
    private static void ConfigureServices(IServiceCollection services, ConfigurationManager configuration,
        IWebHostEnvironment builderEnvironment)
    {
        // DB 컨텍스트 정보를 추가한다.
        string connectionString = configuration.GetConnectionString("AnalysisDatabase") ?? "";
        Console.WriteLine($"[Connection String] {connectionString} [Environment] {builderEnvironment.EnvironmentName}");
        
        // 디버그 환경일 경우 
        if (builderEnvironment.IsDevelopment())
        {
            Console.WriteLine($"[Add SQL Bulk Logging]");
            
            // SQL 쿼리 로그 출력 추가
            services.AddDbContext<AnalysisDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)) 
                    .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information)
                );
        }
        // 그외
        else
        {
            services.AddDbContext<AnalysisDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
        }
        
        // 세션 유지 설정
        services.ConfigureApplicationCookie(options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromMinutes(60*2);
        });
        
        // 구성 빌더 설정
        IConfigurationBuilder builder = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        
        // 데이터 베이스 초기화 여부 설정을 가져온다.
        IConfiguration config = builder.Build();
        bool databaseInitialize = config.GetValue<bool>("DatabaseInitialize");
        
        // 초기화를 해야하는 경우 
        if (databaseInitialize)
        {
            // 데이터 Seed 호스트 서비스 등록 ( 서비스 시작시 구동 )
            services.AddHostedService<SeedDataService>();    
        }
        
        // HttpContext 전역 서비스 레이어에서 사용 
        services.AddHttpContextAccessor();
        
        // 커스텀 Identity 설정 주입
        services.AddLogging();
        // 컨트롤러 설정
        services.AddControllers(controllerConfig =>
        {
            controllerConfig.Filters.Add<CustomValidationFilter>();
        })
        .ConfigureApiBehaviorOptions(options =>
        {
            // 기본 모델 상태 검사를 비활성화
            options.SuppressModelStateInvalidFilter = true;
        });
        services.AddScoped<CustomValidationFilter>();
        
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
        services.AddScoped<IBudgetPlanRepository,BudgetPlanRepository>();
        services.AddScoped<IBudgetApprovedRepository,BudgetApprovedRepository>();
        services.AddScoped<IBudgetProcessRepository,BudgetProcessRepository>();
        services.AddScoped<IBudgetAnalysisCacheRepository,BudgetAnalysisCacheRepository>();
        
        // Add Service Layers
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ISignInService<DbModelUser>, SignInService>();
        services.AddScoped<IBusinessUnitService, BusinessUnitService>();
        services.AddScoped<ISectorService, SectorService>();
        services.AddScoped<ICostCenterService, CostCenterService>();
        services.AddScoped<ILogActionWriteService, LogActionWriteService>();
        services.AddScoped<ILogActionService, LogActionService>();
        services.AddScoped<IBudgetPlanService, BudgetPlanService>();
        services.AddScoped<IBudgetApprovedService, BudgetApprovedService>();
        services.AddScoped<ICountryBusinessManagerService, CountryBusinessManagerService>();
        services.AddScoped<IDispatchService, DispatchService>();
        services.AddScoped<IBudgetProcessService,BudgetProcessService>();
        services.AddScoped<IBudgetAnalysisCacheService,BudgetAnalysisCacheService>();
        services.AddScoped<IExcelService,ExcelService>();
        services.AddScoped<ISystemConfigService,SystemConfigService>();
        services.AddScoped<IFileService,FileService>();
        services.AddScoped<IHashService,HashService>();

        services.AddSingleton<DebounceManager>();
        services.AddTransient<IQueryService, QueryService>();
    }
}

