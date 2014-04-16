using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ImageOrganizer.Web.Startup))]
namespace ImageOrganizer.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
