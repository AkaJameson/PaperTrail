using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using PaperTrail.Model.Manager.Services;
using PaperTrail.Model.Manager.ServicesImpl;
using Si.CoreHub.Package.Entitys;

namespace PaperTrail.Model.Manager
{
    public class Pack : PackBase
    {
        public override void Configuration(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            
        }
        public override void ConfigurationServices(WebApplicationBuilder builder, IServiceCollection services)
        {
            services.AddScoped<ITagService, TagServiceImpl>();
            services.AddScoped<ICategoryService, CategoryServiceImpl>();
            services.AddScoped<ISystemUserService, SystemUserServiceImpl>();
            services.AddScoped<IUploadService, UploadServiceImpl>();
        }
    }
}
