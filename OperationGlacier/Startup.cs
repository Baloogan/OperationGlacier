using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OperationGlacier.Startup))]
namespace OperationGlacier
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
