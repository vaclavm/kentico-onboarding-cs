using System;
using System.Collections.Generic;
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
        private readonly IUnityContainer _unityContainer;
        private const string _webHttpSource = "System.Web.Http";
        private const string _netHttpSource = "System.Net.Http";

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
                when (exception.Message.Contains(_webHttpSource) || exception.Message.Contains(_netHttpSource))
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
                when (exception.Message.Contains(_webHttpSource) || exception.Message.Contains(_netHttpSource))
            {
                return new List<object>();
            }
        }

        public IContainer CreateChildContainer() 
            => new Container(_unityContainer.CreateChildContainer());

        public bool IsRegistered(Type serviceType)
            => _unityContainer.IsRegistered(serviceType);

        public void Dispose()
            => _unityContainer.Dispose();
    }
}
