﻿using System;
using System.Web.Http.Dependencies;

using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace ToDoList.DependencyInjection
{
    public class Wrapper
    {
        private readonly UnityContainer _container = new UnityContainer();

        public IDependencyResolver CreateDependencyResolver() 
            => new UnityResolver(_container);

        public void RegisterType<TFrom, TTo>(LifetimeManager managerType)
            where TTo: TFrom
        {
            switch (managerType)
            {
                case LifetimeManager.Transient:
                    _container.RegisterType<TFrom, TTo>(new TransientLifetimeManager());
                    break;
                default:
                    _container.RegisterType<TFrom, TTo>(new HierarchicalLifetimeManager());
                    break;
            }
            
        }

        public void RegisterType<T>(Func<T> injectionFunction)
        {
            _container.RegisterType<T>(new InjectionFactory(_ => injectionFunction()));
        }
    }
}
