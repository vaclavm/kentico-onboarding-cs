using System;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

using ToDoList.Contracts.DependencyInjection;
using LifetimeManager = ToDoList.Contracts.DependencyInjection.LifetimeManager;

namespace ToDoList.DependencyInjection.Container
{
    internal class Container : IContainer
    {
        private readonly UnityContainer _container = new UnityContainer();

        public object GetContainer()
        {
            return _container;
        }

        public void RegisterType<T>(Func<T> injectionFunction)
            => _container.RegisterType<T>(new InjectionFactory(_ => injectionFunction()));

        public void RegisterType<TFrom, TTo>(LifetimeManager managerType)
            where TTo: TFrom
        {
            switch (managerType)
            {
                case LifetimeManager.Transient:
                    _container.RegisterType<TFrom, TTo>(new TransientLifetimeManager());
                    break;
                case LifetimeManager.Hierarchical:
                    _container.RegisterType<TFrom, TTo>(new HierarchicalLifetimeManager());
                    break;
                case LifetimeManager.Singleton:
                    _container.RegisterType<TFrom, TTo>(new ContainerControlledLifetimeManager());
                    break;
                default:
                    throw new ArgumentException($"Unknown life time manager");
            }
            
        }

        public void RegisterInstance<T>(T instance)
            => _container.RegisterInstance(instance);
    }
}
