using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using PaperTrail.Api.Filter;
using PaperTrail.Storage;
using Si.CoreHub.Extension;
using Si.CoreHub.Logs;
using Si.CoreHub.Utility;
using Si.EntityFramework.Extension.Extensions;
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
            LogCenter.Init(logPath);
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
            builder.Host.ConfiguraSystemHub(logPath);
            //�����־ϵͳ
            builder.Services.UseLogHub();
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
            builder.Services.AddApplicationDbContext<BlogDbContext>(optionAction =>
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
                //������ɾ��
                ExtensionOptionsActio.EnableSoftDelete = true;
            });
            builder.Services.AddUnitofWork<BlogDbContext>();
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

            }).AddJsonOptions(option =>
            {
                option.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            });
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Allows", policy =>
                {
                    policy.AllowAnyMethod()
                      .SetIsOriginAllowed(_ => true)
                      .AllowAnyHeader()
                      .AllowCredentials().WithExposedHeaders("Captcha-Id");
                });
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
            //ģ�����
            var packagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "packages");
            Directory.CreateDirectory(packagePath);
            builder.AddPackages(option =>
            {
                option.FilePath = packagePath;
            });
            var app = builder.Build();
            //���÷���λ��
            ServiceLocator.SetServiceProvider(app.Services);
            //��̬��Դ�ļ�������
            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "uploads"));
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "uploads")),
                RequestPath = "/uploads"
            });
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();  // ���� Swagger �м��
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                    options.RoutePrefix = string.Empty;
                });
            }
            if (ServiceLocator.Configuration.GetValue<bool>("AllowRedirect"))
            {
                app.UseHsts();  // ���� HSTS �м��
                app.UseHttpsRedirection();  // ���� HTTPS �ض����м��
            }
            //�û���Ϣ��������������Routing֮ǰ�����Ȩ����֤�м������ʹ��
            app.UseInfoParser();
            app.UseCors("Allows");
            app.UseRouting();
            //���Ȩ����֤�м��
            app.UseRbacCore<BlogDbContext>();
            //ʹ�ð�
            app.UsePackages(app, app.Services);
            //���IP�����м��
            app.UseRateLimiter();
            app.MapControllers();
            app.Run();
        }
    }
}
