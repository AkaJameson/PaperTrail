using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using PaperTrail.Module.Bloger.Services;
using PaperTrail.Module.Bloger.ServicesImpl;
using Si.CoreHub.Package.Entitys;

namespace PaperTrail.Module.Bloger
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
            services.AddScoped<IUserService, UserServiceImpl>();
            services.AddScoped<IUploadService, UploadServiceImpl>();
            services.AddScoped<IBlogService, BlogServiceImpl>();
            services.AddScoped<ICommentService, CommentServiceImpl>();
        }
    }
}
