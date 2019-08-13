using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EFDatabaseFirst.Startup))]
namespace EFDatabaseFirst
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
