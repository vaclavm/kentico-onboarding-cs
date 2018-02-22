using System.Web.Http;
using ToDoList.API.DependencyInjection;
using ToDoList.Contracts;
using ToDoList.DependencyInjection;

namespace ToDoList.API
{
    internal static class DependencyInjectionConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var diWrapper = new Wrapper();

            Register<Repository.DependencyRegister>(diWrapper);
            Register<DependencyRegister>(diWrapper);

            config.DependencyResolver = new DependencyResolver(diWrapper.GetContainer());
        }

        private static void Register<TRegister>(Wrapper wrapper)
            where TRegister : IDependencyRegister, new()
        {
            var register = new TRegister();
            register.Register(wrapper);
        }
    }
}