using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Week4.Startup))]
namespace Week4
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
