using System;
using System.Collections.Generic;
using Unity;
using Unity.Exceptions;

namespace ToDoList.DependencyInjection
{
    internal class Container : IContainer
    {
        protected IUnityContainer container;

        public Container(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }
            this.container = container;
        }

        public object Resolve(Type serviceType)
        {
            try
            {
                return container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        public IEnumerable<object> ResolveAll(Type serviceType)
        {
            try
            {
                return container.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return new List<object>();
            }
        }

        public T CreateChildContainer<T>()
            where T : new()
        {
            var child = container.CreateChildContainer();
            return (T)Activator.CreateInstance(typeof(T), child);
        }

        public void Dispose()
        {
            container?.Dispose();
        }
    }
}
