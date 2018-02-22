using System.Web.Http;

using ToDoList.API.DependencyInjection;
using ToDoList.API.Helpers;
using ToDoList.Contracts.Services;

namespace ToDoList.API
{
    internal static class DependencyInjectionConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var diBootstrap = new DependencyBootstraper();
            diBootstrap.RegisterDependencies();
            diBootstrap.RegisterApiSingleton<IRoutesService, RoutesHelper>();

            config.DependencyResolver = diBootstrap.CreateResolver();
        }
    }
}