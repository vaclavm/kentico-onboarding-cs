using System.Web.Http;
using Unity;

using ToDoList.API.DI;

namespace ToDoList.API
{
    public static class DependencyConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer();

            var repositoryDiRegister = new Repository.DependencyRegister();
            repositoryDiRegister.Register(container);

            var servicesDiRegister = new DependencyRegister();
            servicesDiRegister.Register(container);

            config.DependencyResolver = new UnityResolver(container);
        }
    }
}