using System;

using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace ToDoList.DependencyInjection
{
    public class Container
    {
        private readonly UnityContainer _container = new UnityContainer();

        public IUnityContainer GetContainer()
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
                    _container.RegisterType<TFrom, TTo>();
                    break;
            }
            
        }

        public void RegisterInstance<T>(T instance)
            => _container.RegisterInstance(instance);
    }
}
