using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QEMS.Startup))]
namespace QEMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
