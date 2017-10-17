using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CBS.Web.Startup))]
namespace CBS.Web
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
