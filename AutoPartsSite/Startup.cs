using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AutoPartsSite.Startup))]
namespace AutoPartsSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
