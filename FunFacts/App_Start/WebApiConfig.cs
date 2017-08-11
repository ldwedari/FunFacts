using System.Web.Http;
using FunFacts.Filters;

namespace FunFacts
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            GlobalConfiguration.Configuration.Filters.Add(new BoundaryExceptionFilterAttribute());
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            SetupDependencyInjection(config);
        }

        private static void SetupDependencyInjection(HttpConfiguration config)
        {
            
        }
    }
}
