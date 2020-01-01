using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ClientWeb.Startup))]
namespace ClientWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
