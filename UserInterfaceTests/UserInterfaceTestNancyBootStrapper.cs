using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace UserInterfaceTests
{
    public class UserInterfaceTestNancyBootStrapper : DefaultNancyBootstrapper
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