using System;
using System.Collections.Generic;
using System.Linq;

using ToDoList.Contracts.DependencyInjection;

namespace ToDoList.API.DependencyInjection.Tests.Container
{
    internal class DummyContainer : IContainer
    {
        private List<Tuple<string, string>> _registrations = new List<Tuple<string, string>>();

        public IEnumerable<string> ExcludedTypes { get; set; }

        public void RegisterType<T>(Func<T> injectionFunction) 
            => _registrations.Add(Tuple.Create(string.Empty, typeof(T).FullName));

        public void RegisterType<TFrom, TTo>() 
            where TTo : TFrom 
            => _registrations.Add(Tuple.Create(typeof(TFrom).FullName, typeof(TTo).FullName));

        public void RegisterTypeAsSingleton<TFrom, TTo>()
            where TTo : TFrom
            => _registrations.Add(Tuple.Create(typeof(TFrom).FullName, typeof(TTo).FullName));

        public void RegisterInstance<T>(T instance)
            => _registrations.Add(Tuple.Create(typeof(T).FullName, instance.GetType().FullName));

        public object ResolveType(Type serviceType) 
            => _registrations.FirstOrDefault(x => x.Item1 == serviceType.FullName)?.Item2;

        public IEnumerable<object> ResolveTypes(Type serviceType)
        {
            yield return _registrations.Where(x => x.Item1.Contains(serviceType.FullName)).Select(x => x.Item2);
        }

        public IContainer CreateChildContainer() 
            => this;

        public void Dispose() { }
    }
}
