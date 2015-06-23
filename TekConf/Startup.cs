using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TekConf.Startup))]
namespace TekConf
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
