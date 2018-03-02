using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Web.Http.Dependencies;
using Unity;
using Unity.Exceptions;


[assembly: InternalsVisibleTo("ToDoList.API.DependencyInjection.Tests")]

namespace ToDoList.Api.DependencyInjection.Resolver
{
    internal sealed class DependencyResolver : IDependencyResolver
    {
        private bool _disposed;
        private readonly IUnityContainer _unityContainer;
        private const string _webHttpSource = "System.Web.Http";
        
        public DependencyResolver(IUnityContainer container)
        {
            _unityContainer = container ?? throw new ArgumentNullException(nameof(container));
        }

        public bool IsRegistered(Type serviceType)
            => _unityContainer.IsRegistered(serviceType);

        public object GetService(Type serviceType)
        {
            try
            {
                return _unityContainer.Resolve(serviceType);
            }
            catch (ResolutionFailedException exception) when (exception.Message.Contains(_webHttpSource))
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
            catch (ResolutionFailedException exception) when (exception.Message.Contains(_webHttpSource))
            {
                return new List<object>();
            }
        }

        public IDependencyScope BeginScope() 
            => new DependencyResolver(_unityContainer.CreateChildContainer());

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