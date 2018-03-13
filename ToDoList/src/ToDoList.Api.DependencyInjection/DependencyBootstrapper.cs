using System.Web.Http.Dependencies;

using ToDoList.Api.DependencyInjection.Resolver;
using ToDoList.Api.Services.DependencyInjection;
using ToDoList.Contracts.DependencyInjection;
using ToDoList.Contracts.Services;
using ToDoList.DependencyInjection;

namespace ToDoList.Api.DependencyInjection
{
    public class DependencyBootstrapper
    {
        private static IContainer _container;

        public static IDependencyResolver CreateWebApiResolver(IWebApiRoutes webApiRoutes)
            => new DependencyBootstrapper(ContainerFactory.GetContainer())
                .Register<Repository.DependencyInjection.DependencyRegister>()
                .Register<DependencyRegister>()
                .RegisterInstance(webApiRoutes)
                .CreateResolver();

        private DependencyBootstrapper(IContainer container)
        {
            _container = container;
        }

        private UnityDependencyResolver CreateResolver()
            => new UnityDependencyResolver(_container);

        private DependencyBootstrapper RegisterInstance<TInstance>(TInstance instance)
        {
            _container.RegisterInstance(instance);
            return this;
        }

        private DependencyBootstrapper Register<TRegister>()
            where TRegister : IDependencyRegister, new()
        {
            var register = new TRegister();
            register.Register(_container);

            return this;
        }
    }
}
