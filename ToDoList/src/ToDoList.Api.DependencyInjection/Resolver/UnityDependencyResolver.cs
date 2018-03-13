using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Unity;
using Unity.Exceptions;

using ToDoList.Contracts.DependencyInjection;

namespace ToDoList.Api.DependencyInjection.Resolver
{
    internal sealed class UnityDependencyResolver : IDependencyResolver
    {
        private bool _disposed;

        private readonly IUnityContainer _unityContainer;

        public UnityDependencyResolver(IContainer container)
        {
            try
            {
                _unityContainer = (IUnityContainer)container.GetContainer();
            }
            catch
            {
                throw new ArgumentException(nameof(container));
            }
        }

        private UnityDependencyResolver(IUnityContainer container)
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
            return new UnityDependencyResolver(child);
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