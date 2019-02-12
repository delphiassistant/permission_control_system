using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IdentityCustomized.Startup))]
namespace IdentityCustomized
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
