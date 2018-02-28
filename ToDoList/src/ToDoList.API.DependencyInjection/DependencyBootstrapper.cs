using System.Web.Http.Dependencies;

using ToDoList.Contracts;
using ToDoList.Contracts.Services;
using ToDoList.DependencyInjection;

namespace ToDoList.API.DependencyInjection
{
    public class DependencyBootstrapper
    {
        private static Container _container;

        public static IDependencyResolver CreateWebApiResolver(IWebApiRoutes webApiRoutes)
            => new DependencyBootstrapper(new Container())
                .Register<Repository.DependencyRegister>()
                .Register<Services.DependencyRegister>()
                .RegisterInstance(webApiRoutes)
                .CreateResolver();

        internal DependencyBootstrapper(Container container)
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
