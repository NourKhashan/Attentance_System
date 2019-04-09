using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AttentanceManagementSystem.Startup))]
namespace AttentanceManagementSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
