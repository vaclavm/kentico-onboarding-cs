using System.Web.Http.Dependencies;

using ToDoList.Api.DependencyInjection.Resolver;
using ToDoList.Contracts.Services;
using ToDoList.DependencyInjection;
using ToDoList.DependencyInjection.Container;

namespace ToDoList.Api.DependencyInjection
{
    public class DependencyBootstrapper
    {
        private static Container _container;

        public static IDependencyResolver CreateWebApiResolver(IWebApiRoutes webApiRoutes)
            => new DependencyBootstrapper(new Container())
                .Register<ToDoList.Repository.DependencyInjection.DependencyRegister>()
                .Register<ToDoList.Services.DependencyInjection.DependencyRegister>()
                .Register<Api.Services.DependencyInjection.DependencyRegister>()
                .RegisterInstance(webApiRoutes)
                .CreateResolver();
        
        private DependencyBootstrapper(Container container)
        {
            _container = container;
        }

        private DependencyResolver CreateResolver()
            => new DependencyResolver(_container.GetContainer());

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
