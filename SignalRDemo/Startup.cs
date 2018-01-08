using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SignalRDemo.Startup))]
namespace SignalRDemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
