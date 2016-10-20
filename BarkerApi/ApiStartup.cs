using Owin;

namespace BarkerApi
{
    public class ApiStartup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            appBuilder.UseNancy();
        }
    }
}