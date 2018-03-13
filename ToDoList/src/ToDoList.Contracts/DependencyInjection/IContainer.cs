using System;

namespace ToDoList.Contracts.DependencyInjection
{
    public interface IContainer
    {
        object GetContainer();

        void RegisterType<T>(Func<T> injectionFunction);

        void RegisterType<TFrom, TTo>(LifetimeManager managerType) where TTo : TFrom;

        void RegisterInstance<T>(T instance);
    }
}
