using System;
using System.Collections.Generic;

namespace ToDoList.DependencyInjection
{
    public interface IContainer : IDisposable
    {
        object Resolve(Type serviceType);

        IEnumerable<object> ResolveAll(Type serviceType);

        T CreateChildContainer<T>() where T : new();
    }
}
