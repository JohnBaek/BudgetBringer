using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models.DataModels;

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

        //
        // // Identity 서비스 DI 
        // services.AddIdentity<User, IdentityRole>()
        //     .AddEntityFrameworkStores<AnalysisDbContext>()
        //     .AddDefaultTokenProviders();
        //
        // // 프로바이더 DI
        // services.AddTransient<ILoginService, LoginProvider>();
        //
        
        services.AddLogging();
        services.AddControllers();
        // 인증서비스 DI
        services.AddAuthentication();
        // 컨트롤러 추가
        services.AddControllers();
        // 빌더에 미들웨어 서비스를 추가한다.
        services.AddEndpointsApiExplorer();
    }
}