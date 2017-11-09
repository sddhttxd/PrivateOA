using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PrivateOA.Web.Startup))]
namespace PrivateOA.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
