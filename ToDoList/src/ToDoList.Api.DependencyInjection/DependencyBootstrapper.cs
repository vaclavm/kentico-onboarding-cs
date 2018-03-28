using System.Web.Http.Dependencies;

using ToDoList.Api.DependencyInjection.Resolver;
using ToDoList.Contracts.DependencyInjection;
using ToDoList.Contracts.Providers;
using ToDoList.DependencyInjection;

namespace ToDoList.Api.DependencyInjection
{
    public class DependencyBootstrapper
    {
        private readonly IContainer _container;
        
        public DependencyBootstrapper() : this(ContainerFactory.GetContainer()) { }

        internal DependencyBootstrapper(IContainer container) 
            => _container = container;

        public IDependencyResolver CreateWebApiResolver(IWebApiRoutes webApiRoutes)
            => Register<ToDoList.Repository.DependencyRegister>()
            .Register<ToDoList.Services.DependencyRegister>()
            .Register<ToDoList.Api.Services.DependencyRegister>()
            .RegisterInstance(webApiRoutes)
            .CreateResolver();

        private DependencyResolver CreateResolver()
            => new DependencyResolver(_container);

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
