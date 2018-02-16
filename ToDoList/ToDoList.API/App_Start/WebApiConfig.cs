using System.Web.Http;
using ToDoList.API.DI;
using ToDoList.Contracts.Repositories;
using ToDoList.Repository;
using Unity;
using Unity.Lifetime;

namespace ToDoList.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var container = new UnityContainer();
            container.RegisterType<IToDoRepository, ToDoRepository>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        public static void Configuration(HttpConfiguration configuration)
        {
            configuration.AddApiVersioning();
        }
    }
}
