using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace BarkerApi
{
    public class CorsBootStrapper : DefaultNancyBootstrapper
    {
        public static bool Initialised = false;
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            if (Initialised) return;
            Initialised = true;
            pipelines.AfterRequest += ctx =>
            {
                ctx.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                ctx.Response.Headers.Add("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept, Authorization");
                ctx.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, DELETE");
            };
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            container.Register<INancyModuleCatalog>(new ApiControllerCatalog());
        }
    }
}
