using Autofac;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Whispir.Messaging.SDK;

namespace Whispir.Messaging.WebSample
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MessageService>()
            .As<IMessageService>()
            .WithParameter("settings", new WhispirSettings()
            {
                ApiAuthorization = WebConfig.WhispirAuthorization,
                ApiBaseUrl = WebConfig.WhispirApiUrl,
                ApiKey = WebConfig.WhispirApiKey,
                LoggingFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data"),
                DataBaseFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data"),
                LoggingHours = WebConfig.LoggingHours
            }).InstancePerLifetimeScope(); 

            

            // Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

        

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
    public static class WebConfig
    {
        public static string WhispirApiKey = ConfigurationManager.AppSettings["WhispirApiKey"];
        public static string WhispirApiUrl = ConfigurationManager.AppSettings["WhispirApiUrl"];
        public static string WhispirAuthorization = ConfigurationManager.AppSettings["WhispirApiAuthorization"];
        public static int LoggingHours = Convert.ToInt32(ConfigurationManager.AppSettings["LoggingHours"]);
    }
}
