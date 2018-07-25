using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Hosting;
using System.Web.Http.Metadata;
using System.Web.Http.Validation;

using ToDoList.Contracts.DependencyInjection;
using ToDoList.Contracts.Exceptions;

namespace ToDoList.Api.DependencyInjection.Resolver
{
    internal sealed class DependencyResolver : IDependencyResolver
    {
        private bool _disposed;
        private readonly IContainer _container;
        private readonly HashSet<string> _excludedDependencies = new HashSet<string>
        {
            typeof(IHostBufferPolicySelector).FullName,
            typeof(IHttpControllerSelector).FullName,
            typeof(IHttpControllerActivator).FullName,
            typeof(IHttpActionSelector).FullName,
            typeof(IHttpActionInvoker).FullName,
            typeof(IContentNegotiator).FullName,
            typeof(IExceptionHandler).FullName,
            typeof(IModelValidatorCache).FullName,
            typeof(ModelMetadataProvider).FullName,
        };

        internal DependencyResolver(IContainer container) 
            => _container = container ?? throw new ArgumentNullException(nameof(container));

        public object GetService(Type serviceType)
        {
            try
            {
                return _container.ResolveType(serviceType);
            }
            catch (DependencyResolutionException)
                when (IsExcludedType(serviceType.FullName))
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return _container.ResolveTypes(serviceType);
            }
            catch (DependencyResolutionException)
                when(IsExcludedType(serviceType.FullName))
            {
                return Enumerable.Empty<object>();
            }
        }

        public IDependencyScope BeginScope() 
            => new DependencyResolver(_container.CreateChildContainer());

        private bool IsExcludedType(string exceptionType)
            => _excludedDependencies.Contains(exceptionType);

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _container.Dispose();
            _disposed = true;
        }
    }
}