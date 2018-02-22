using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

using ToDoList.DependencyInjection;

namespace ToDoList.API.DependencyInjection
{
    public class DependencyResolver : IDependencyResolver
    {
        protected IContainer container;

        public DependencyResolver() { }

        public DependencyResolver(IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }
            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            return container.Resolve(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return container.ResolveAll(serviceType);
        }

        public IDependencyScope BeginScope()
        {
            return container.CreateChildContainer<DependencyResolver>();
        }

        public void Dispose() => container.Dispose();
    }
}