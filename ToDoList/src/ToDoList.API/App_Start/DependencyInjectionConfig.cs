using System.Web.Http;

using ToDoList.DependencyInjection;

namespace ToDoList.API
{
    internal static class DependencyInjectionConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var diWrapper = new Wrapper();

            var repositoryDiRegister = new Repository.DependencyRegister();
            repositoryDiRegister.Register(diWrapper);

            var servicesDiRegister = new DependencyRegister();
            servicesDiRegister.Register(diWrapper);

            config.DependencyResolver = diWrapper.CreateDependencyResolver();
        }
    }
}