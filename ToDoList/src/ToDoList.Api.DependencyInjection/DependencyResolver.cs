using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Web.Http.Dependencies;
using Unity;
using Unity.Exceptions;

[assembly: InternalsVisibleTo("ToDoList.API.DependencyInjection.Tests")]

namespace ToDoList.Api.DependencyInjection
{
    internal sealed class DependencyResolver : IDependencyResolver
    {
        private bool _disposed;

        private readonly IUnityContainer _unityContainer;
        
        public DependencyResolver(IUnityContainer container)
        {
            _unityContainer = container ?? throw new ArgumentNullException(nameof(container));
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return _unityContainer.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return _unityContainer.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return new List<object>();
            }
        }

        public IDependencyScope BeginScope()
        {
            var child = _unityContainer.CreateChildContainer();
            return new DependencyResolver(child);
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _unityContainer.Dispose();
            _disposed = true;
        }
    }
}