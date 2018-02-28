using System.Web.Http;
using ToDoList.API.DependencyInjection;
using ToDoList.API.Helpers;

namespace ToDoList.Api
{
    internal static class DependencyInjectionConfig
    {
        public static void Register(HttpConfiguration config)
            => config.DependencyResolver = DependencyBootstrapper.CreateWebApiResolver(new WebApiRoutes());
    }
}