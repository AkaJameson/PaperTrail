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
            //日志文件路径
            var logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "logs");
            //日志文件配置
            LogCenter.Init(logPath);
            //启动
            var builder = WebApplication.CreateBuilder(args);
            //Configuration文件配置
            builder.Configuration.Sources.Clear();
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            //定位器添加
            ServiceLocator.Configuration = builder.Configuration;
            //清除默认日志提供者
            builder.Logging.ClearProviders();
            //添加自定义日志提供者（输出框架级别日志）
            builder.Host.ConfiguraSystemHub(logPath);
            //添加日志系统
            builder.Services.UseLogHub();
            //添加内存事件总线
            builder.Services.AddInMemoryEventBus();
            //使用内存缓存机制
            builder.Services.UseMemoryCache();
            //useSwagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            //添加Context获取器
            builder.Services.AddHttpContextAccessor();
            //配置服务器
            builder.UseKestrel();
            //配置数据库
            var connectionStr = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddApplicationDbContext<BlogDbContext>(optionAction =>
            {
                optionAction.UseMySql(connectionStr, ServerVersion.AutoDetect(connectionStr));
                //懒加载
                optionAction.UseLazyLoadingProxies();
            }, ExtensionOptionsActio =>
            {
                //不启动雪花
                ExtensionOptionsActio.EnableSnowflakeId = false;
                //启动审计
                ExtensionOptionsActio.EnableAudit = true;
                //启动软删除
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
            //添加控制器
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
            //添加IP限流
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
            //添加HSTS中间件
            builder.Services.AddHsts(options =>
            {
                options.MaxAge = TimeSpan.FromDays(365);  // 设置 HSTS 过期时间
                options.IncludeSubDomains = true;  // 包括子域名
                options.Preload = true;  // 启用预加载
            });
            //模块加载
            var packagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "packages");
            Directory.CreateDirectory(packagePath);
            builder.AddPackages(option =>
            {
                option.FilePath = packagePath;
            });
            var app = builder.Build();
            //设置服务定位器
            ServiceLocator.SetServiceProvider(app.Services);
            //静态资源文件夹设置
            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "uploads"));
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "uploads")),
                RequestPath = "/uploads"
            });
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();  // 启用 Swagger 中间件
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                    options.RoutePrefix = string.Empty;
                });
            }
            if (ServiceLocator.Configuration.GetValue<bool>("AllowRedirect"))
            {
                app.UseHsts();  // 启用 HSTS 中间件
                app.UseHttpsRedirection();  // 启用 HTTPS 重定向中间件
            }
            //用户信息解析器（必须在Routing之前）配合权限验证中间件进行使用
            app.UseInfoParser();
            app.UseCors("Allows");
            app.UseRouting();
            //添加权限验证中间件
            app.UseRbacCore<BlogDbContext>();
            //使用包
            app.UsePackages(app, app.Services);
            //添加IP限流中间件
            app.UseRateLimiter();
            app.MapControllers();
            app.Run();
        }
    }
}
