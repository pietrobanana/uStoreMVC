using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(uStoreMVC.Startup))]
namespace uStoreMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
