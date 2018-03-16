using System;
using System.Collections.Generic;

namespace ToDoList.Contracts.DependencyInjection
{
    public interface IContainer : IDisposable
    {
        IEnumerable<string> ExcludedTypes { get; set; }

        void RegisterType<T>(Func<T> injectionFunction);

        void RegisterType<TFrom, TTo>(LifetimeManager managerType) where TTo : TFrom;

        void RegisterInstance<T>(T instance);

        object ResolveType(Type serviceType);

        IEnumerable<object> ResolveTypes(Type serviceType);

        IContainer CreateChildContainer();
    }
}
