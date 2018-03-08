using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AMP.Startup))]
namespace AMP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           // ConfigureAuth(app);
        }
    }
}
