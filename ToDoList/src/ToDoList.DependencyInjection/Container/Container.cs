using System;
using System.Collections.Generic;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.Exceptions;

using ToDoList.Contracts.DependencyInjection;
using ToDoList.Contracts.Exceptions;

namespace ToDoList.DependencyInjection.Container
{
    internal class Container : IContainer
    {
        private bool _disposed;
        private readonly IUnityContainer _unityContainer;

        public Container() : this(new UnityContainer()) { }

        private Container(IUnityContainer unityContainer)
            => _unityContainer = unityContainer;

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
            {
                throw new DependencyResolutionException(exception.Message, exception);
            }
        }

        public IEnumerable<object> ResolveTypes(Type serviceType)
        {
            try
            {
                return _unityContainer.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException exception)
            {
                throw new DependencyResolutionException(exception.Message, exception);
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
    }
}
