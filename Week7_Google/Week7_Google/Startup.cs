using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Week7_Google.Startup))]
namespace Week7_Google
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
