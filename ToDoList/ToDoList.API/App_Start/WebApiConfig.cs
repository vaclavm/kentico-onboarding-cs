using System.Web.Http;
using System.Web.Http.Routing;
using Microsoft.Web.Http.Routing;
using Unity;

using ToDoList.API.DI;


namespace ToDoList.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API DI
            config.DependencyResolver = CreateUnityResolver(); 

            // Web API versioning
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

        private static UnityResolver CreateUnityResolver()
        {
            var container = new UnityContainer();

            var repositoryDiRegister = new Repository.DependencyRegister();
            repositoryDiRegister.Register(container);

            var servicesDiRegister = new DependencyRegister();
            servicesDiRegister.Register(container);

            return new UnityResolver(container); 
        }
    }
}
