using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using PaperTrail.Api.Filter;
using PaperTrail.Api.Models;
using PaperTrail.Storage;
using Si.CoreHub.Extension;
using Si.CoreHub.Logs;
using Si.CoreHub.Utility;
using Si.EntityFramework.Extension;
using Si.EntityFramework.Extension.Abstraction;
using Si.EntityFramework.Extension.UnitofWork;
using Si.EntityFramework.PermGuard.Extensions;
using System.Threading.RateLimiting;

namespace PaperTrail.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //��־�ļ�·��
            var logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "logs");
            //��־�ļ�����
            LogSetting.Init(logPath);
            //����
            var builder = WebApplication.CreateBuilder(args);
            //Configuration�ļ�����
            builder.Configuration.Sources.Clear();
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            //��λ�����
            ServiceLocator.Configuration = builder.Configuration;
            //���Ĭ����־�ṩ��
            builder.Logging.ClearProviders();
            //����Զ�����־�ṩ�ߣ������ܼ�����־��
            builder.Host.UseLog(logPath);
            //����ڴ��¼�����
            builder.Services.AddInMemoryEventBus();
            //ʹ���ڴ滺�����
            builder.Services.UseMemoryCache();
            //useSwagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            //���Context��ȡ��
            builder.Services.AddHttpContextAccessor();
            //���÷�����
            builder.UseKestrel();
            //�������ݿ�
            var connectionStr = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddSiDbContext<BlogDbContext>(optionAction =>
            {
                optionAction.UseMySql(connectionStr, ServerVersion.AutoDetect(connectionStr));
                //������
                optionAction.UseLazyLoadingProxies();
            }, ExtensionOptionsActio =>
            {
                //������ѩ��
                ExtensionOptionsActio.EnableSnowflakeId = false;
                //�������
                ExtensionOptionsActio.EnableAudit = true;
                //��������ɾ��
                ExtensionOptionsActio.EnableSoftDelete = false;
            });
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork<BlogDbContext>>();
            builder.Services.AddRbacCore(option =>
            {
                option.ConfigPath = "RbacConfig.xml";
                option.Audience = builder.Configuration.GetValue<string>("JwtSettings:Secret") ?? "bLeerei5igQ5x2ffqwGqjm3CJ4nsBCmI";
                option.Issuer = builder.Configuration.GetValue<string>("JwtSettings:Issuer") ?? "http://localhost:5000";
                option.Audience = builder.Configuration.GetValue<string>("JwtSettings:Audience") ?? "http://localhost:5000";
            });
            //��ӿ�����
            builder.Services.AddControllers(option =>
            {
                option.Filters.Add<GlobalExceptionFilter>();

            }).AddJsonOptions(option => { 
                option.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull; 
            });
            //���CurrentUser��ȡ��
            builder.Services.AddCurrentUserAccessor((privider) =>
            {
                var httpContext = privider.GetService<HttpContextAccessor>();
                var Id = httpContext?.HttpContext?.Items["Id"]?.ToString();
                return Id == null ? null : new CurrentUser { UserId = Id };
            });
            //���IP����
            builder.Services.AddRateLimiter(options =>
            {
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        context.Request.Headers.UserAgent.ToString(),
                        _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 100,
                            Window = TimeSpan.FromMinutes(5)
                        }));

                options.AddFixedWindowLimiter("API", limiterOptions =>
                {
                    limiterOptions.PermitLimit = 30;
                    limiterOptions.Window = TimeSpan.FromMinutes(1);
                });
                options.OnRejected = (context, _) =>
                {
                    context.HttpContext.Response.StatusCode = 429;
                    context.HttpContext.Response.WriteAsync("Too many requests, please try again later.");
                    return ValueTask.CompletedTask;
                };
            });
            //���HSTS�м��
            builder.Services.AddHsts(options =>
            {
                options.MaxAge = TimeSpan.FromDays(365);  // ���� HSTS ����ʱ��
                options.IncludeSubDomains = true;  // ����������
                options.Preload = true;  // ����Ԥ����
            });
            var packagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "packages");
            Directory.CreateDirectory(packagePath);
            builder.AddPackages(option =>
            {
                option.FilePath = packagePath;
            });
            var app = builder.Build();
            ServiceLocator.SetServiceProvider(app.Services);
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();  // ���� Swagger �м��
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                });
            }

            // ʹ�� HSTS ���ã�ֻ��������������ʹ�� HTTPS��
            if (app.Environment.IsProduction())
            {
                app.UseHsts();  // ���� HSTS �м��
                app.UseHttpsRedirection();  // ���� HTTPS �ض����м��
            }
            //���Ȩ����֤�м��
            app.UseRbacCore<BlogDbContext>();
            app.UseRouting();
            //���IP�����м��
            app.UseRateLimiter();
            app.UsePackages(app, app.Services);
            app.MapControllers();
            app.Run();
        }
    }
}
