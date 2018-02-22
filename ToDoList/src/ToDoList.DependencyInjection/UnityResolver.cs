﻿using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

using Unity;
using Unity.Exceptions;

namespace ToDoList.DependencyInjection
{
    public class UnityResolverX : IDependencyResolver
    {
        protected IUnityContainer container;

        public UnityResolverX(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }
            this.container = container;
        }

        public object GetService(Type serviceType)
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

        public IEnumerable<object> GetServices(Type serviceType)
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

        public IDependencyScope BeginScope()
        {
            var child = container.CreateChildContainer();
            return new UnityResolverX(child);
        }

        public void Dispose() => container.Dispose();
    }
}