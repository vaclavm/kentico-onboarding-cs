using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

using ToDoList.Contracts.DependencyInjection;

namespace ToDoList.Api.DependencyInjection.Resolver
{
    internal sealed class DependencyResolver : IDependencyResolver
    {
        private bool _disposed;
        private readonly IContainer _container;

        internal DependencyResolver(IContainer container) 
            => _container = container ?? throw new ArgumentNullException(nameof(container));

        public object GetService(Type serviceType)
            => _container.ResolveType(serviceType);

        public IEnumerable<object> GetServices(Type serviceType)
            => _container.ResolveTypes(serviceType);

        public IDependencyScope BeginScope() 
            => new DependencyResolver(_container.CreateChildContainer());

        public bool IsRegistered(Type serviceType)
            => _container.IsRegistered(serviceType);

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