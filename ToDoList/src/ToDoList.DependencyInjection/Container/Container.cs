using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Hosting;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

using ToDoList.Contracts.DependencyInjection;
using Unity.Exceptions;
using LifetimeManager = ToDoList.Contracts.DependencyInjection.LifetimeManager;

namespace ToDoList.DependencyInjection.Container
{
    internal class Container : IContainer
    {
        private bool _disposed;
        private readonly IUnityContainer _unityContainer;
        private static IEnumerable<string> _unresolvableTypes = new List<string>
        {
            nameof(IHostBufferPolicySelector),
            nameof(IHttpControllerSelector),
            nameof(IHttpControllerActivator),
            nameof(IHttpActionSelector),
            nameof(IHttpActionInvoker),
            nameof(IContentNegotiator),
            nameof(IExceptionHandler)
        };

        public Container() : this(new UnityContainer()) { }

        private Container(IUnityContainer unityContainer)
            => _unityContainer = unityContainer;

        public void RegisterType<T>(Func<T> injectionFunction)
            => _unityContainer.RegisterType<T>(new InjectionFactory(_ => injectionFunction()));

        public void RegisterType<TFrom, TTo>(LifetimeManager managerType)
            where TTo: TFrom
        {
            switch (managerType)
            {
                case LifetimeManager.Transient:
                    _unityContainer.RegisterType<TFrom, TTo>(new TransientLifetimeManager());
                    break;
                case LifetimeManager.Hierarchical:
                    _unityContainer.RegisterType<TFrom, TTo>(new HierarchicalLifetimeManager());
                    break;
                case LifetimeManager.Singleton:
                    _unityContainer.RegisterType<TFrom, TTo>(new ContainerControlledLifetimeManager());
                    break;
                default:
                    throw new ArgumentException($"Unknown life time manager");
            }
            
        }

        public void RegisterInstance<T>(T instance)
            => _unityContainer.RegisterInstance(instance);

        public object ResolveType(Type serviceType)
        {
            try
            {
                return _unityContainer.Resolve(serviceType);
            }
            catch (ResolutionFailedException exception) 
                when (IsWebOrNetException(exception.TypeRequested))
            {
                return null;
            }
        }

        public IEnumerable<object> ResolveTypes(Type serviceType)
        {
            try
            {
                return _unityContainer.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException exception)
                when (IsWebOrNetException(exception.TypeRequested))
            {
                return new List<object>();
            }
        }

        public IContainer CreateChildContainer() 
            => new Container(_unityContainer.CreateChildContainer());

        public bool IsRegistered(Type serviceType)
            => _unityContainer.IsRegistered(serviceType);
        
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _unityContainer.Dispose();
            _disposed = true;
        }

        private bool IsWebOrNetException(string exceptionType) 
            => _unresolvableTypes.Contains(exceptionType);
    }
}
