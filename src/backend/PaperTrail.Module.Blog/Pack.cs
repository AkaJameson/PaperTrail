using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Si.CoreHub.Package.Entitys;

namespace PaperTrail.Module.Blog
{
    public class Pack : PackBase
    {
        public override void Configuration(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {

        }

        public override void ConfigurationServices(WebApplicationBuilder builder, IServiceCollection services)
        {

        }
    }
}
