using System.Web.Http;
using System.Web.Http.Routing;
using Microsoft.Web.Http.Routing;


namespace ToDoList.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API versioning
            var constraintResolver = new DefaultInlineConstraintResolver
            {
                ConstraintMap =
                {
                    ["apiVersion"] = typeof(ApiVersionRouteConstraint)
                }
            };
            
            config.MapHttpAttributeRoutes(constraintResolver);
            config.AddApiVersioning();
        }
    }
}
