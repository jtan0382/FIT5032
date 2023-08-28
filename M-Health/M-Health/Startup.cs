using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(M_Health.Startup))]
namespace M_Health
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
