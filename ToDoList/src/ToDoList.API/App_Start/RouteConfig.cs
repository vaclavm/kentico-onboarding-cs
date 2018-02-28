using System.Web.Http;
using System.Web.Http.Routing;
using Microsoft.Web.Http.Routing;


namespace ToDoList.API
{
    internal static class RouteConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Route configuration with ApiVersioning
            config.MapHttpAttributeRoutes(CreateConstrainResolver());
            config.AddApiVersioning();
        }

        private static IInlineConstraintResolver CreateConstrainResolver()
        {
            var constraintResolver = new DefaultInlineConstraintResolver
            {
                ConstraintMap =
                {
                    ["apiVersion"] = typeof(ApiVersionRouteConstraint)
                }
            };

            return constraintResolver;
        }
    }
}
