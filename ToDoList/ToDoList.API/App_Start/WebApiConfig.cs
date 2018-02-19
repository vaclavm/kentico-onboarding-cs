using System.Web.Http;
using System.Web.Http.Routing;
using Microsoft.Web.Http.Routing;

namespace ToDoList.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API constrains
            var constraintResolver = new DefaultInlineConstraintResolver
            {
                ConstraintMap =
                {
                    ["apiVersion"] = typeof(ApiVersionRouteConstraint)
                }
            };

            // Web API routes
            config.MapHttpAttributeRoutes(constraintResolver);
        }

        public static void Configuration(HttpConfiguration configuration)
        {
            configuration.AddApiVersioning();
        }
    }
}
