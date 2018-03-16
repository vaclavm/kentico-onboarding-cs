using System.Web.Http;
using ToDoList.Api.DependencyInjection;
using ToDoList.API.Helpers;

namespace ToDoList.Api
{
    internal static class DependencyInjectionConfig
    {
        public static void Register(HttpConfiguration config)
            => config.DependencyResolver = new DependencyBootstrapper().CreateWebApiResolver(new WebApiRoutes());
    }
}