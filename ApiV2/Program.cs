using System.Configuration;
using System.Reflection;
using Features.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Models.DataModels;

namespace ApiV2;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();


        // builder.Services.AddDbContext<AnalysisDbContext>(options =>
        //     options.UseMySql(builder.Configuration.GetConnectionString("AnalysisDatabase"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("AnalysisDatabase"))));
        //
        // // HttpContext 전역 서비스 레이어에서 사용
        // builder.Services.AddHttpContextAccessor();
        // // 컨트롤러 설정
        // builder.Services.AddControllers();
        // builder.Services.AddAuthentication();
        // services.AddEndpointsApiExplorer();

        // // Reverse Proxy 설정
        // builder.Services.Configure<ForwardedHeadersOptions>(options =>
        // {
        //     options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        // });
        //
        // 스웨거 설정
        builder.Services.AddSwaggerGen(swagger =>
        {
            swagger.SwaggerDoc("v2" , new OpenApiInfo()
            {
                Version = "v2"
                , Title = "BudgetBringer API"
                , Description = ""
            });

            swagger.OperationFilter<IgnorePropertyFilter>();

            // XML 주석 파일 설정
            string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            swagger.IncludeXmlComments(xmlPath);
        });

        // builder.Services.AddHostedService<Worker>();

        var app = builder.Build();

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
        //
        // // File RealPath
        // string staticFileDirectory = app.Environment.IsDevelopment() ?
        //     "/Users/john/Library/Caches/Budget/Persist" :
        //     "/data-files/file-persist";
        //
        // app.UseStaticFiles(new StaticFileOptions
        // {
        //     FileProvider = new PhysicalFileProvider(staticFileDirectory),
        //     // URL root path , should configured in nginx
        //     RequestPath =  "/files"
        // });
        //
        // app.UseAuthentication();
        // app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}