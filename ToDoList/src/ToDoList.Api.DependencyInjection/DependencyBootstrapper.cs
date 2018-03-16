using System.Collections.Generic;
using System.Net.Http.Formatting;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Hosting;

using ToDoList.Api.DependencyInjection.Resolver;
using ToDoList.Contracts.DependencyInjection;
using ToDoList.Contracts.Providers;
using ToDoList.Contracts.Services;
using ToDoList.DependencyInjection;

namespace ToDoList.Api.DependencyInjection
{
    public class DependencyBootstrapper
    {
        private readonly IContainer _container;
        
        public DependencyBootstrapper() : this(ContainerFactory.GetContainer()) { }

        internal DependencyBootstrapper(IContainer container)
        {
            _container = container;

            _container.ExcludedTypes = new List<string>
            {
                nameof(IHostBufferPolicySelector),
                nameof(IHttpControllerSelector),
                nameof(IHttpControllerActivator),
                nameof(IHttpActionSelector),
                nameof(IHttpActionInvoker),
                nameof(IContentNegotiator),
                nameof(IExceptionHandler)
            };
        }

        public IDependencyResolver CreateWebApiResolver(IWebApiRoutes webApiRoutes)
        {
            return Register<ToDoList.Repository.DependencyRegister>()
                .Register<ToDoList.Services.DependencyRegister>()
                .Register<ToDoList.Api.Services.DependencyRegister>()
                .RegisterInstance(webApiRoutes)
                .CreateResolver();
        }

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
