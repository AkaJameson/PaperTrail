using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using PaperTrail.Model.Login.Services;
using PaperTrail.Model.Login.ServicesImpl;
using PaperTrail.Utilities.Captcha;
using Si.CoreHub.Package.Core;
using Si.CoreHub.Package.Entitys;

namespace PaperTrail.Model.Login
{
    public class Pack : PackBase
    {
        
        public override void ConfigurationServices(WebApplicationBuilder builder, IServiceCollection services)
        {
            var configuration = PackConfigurationProvider.GetConfiguration("PaperTrail.Model.Login");
            services.AddCaptcha(option =>
            {
                option.NoiseCount = 5;
                option.ExpirationSeconds = 300;
            });
            services.AddScoped<ILoginService, LoginServiceImpl>();
        }
        public override void Configuration(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {

        }
    }
}
