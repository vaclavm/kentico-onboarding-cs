using System;
using System.Collections.Generic;
using System.Linq;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.Exceptions;

using ToDoList.Contracts.DependencyInjection;

namespace ToDoList.DependencyInjection.Container
{
    internal class Container : IContainer
    {
        private bool _disposed;
        private readonly IUnityContainer _unityContainer;

        public Container() : this(new UnityContainer()) { }

        private Container(IUnityContainer unityContainer)
            => _unityContainer = unityContainer;

        public IEnumerable<string> ExcludedTypes { get; set; }

        public void RegisterType<T>(Func<T> injectionFunction)
            => _unityContainer.RegisterType<T>(new InjectionFactory(_ => injectionFunction()));

        public void RegisterType<TFrom, TTo>()
            where TTo: TFrom 
            => _unityContainer.RegisterType<TFrom, TTo>(new ContainerControlledLifetimeManager());

        public void RegisterTypeAsSingleton<TFrom, TTo>()
            where TTo : TFrom 
            => _unityContainer.RegisterType<TFrom, TTo>(new HierarchicalLifetimeManager());
        
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
            => ExcludedTypes.Contains(exceptionType);
    }
}
