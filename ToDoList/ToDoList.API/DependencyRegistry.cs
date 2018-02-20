using System.Net.Http;
using System.Web;
using Unity;
using Unity.Lifetime;
using Unity.Injection;

using ToDoList.Contracts;
using ToDoList.Contracts.Services;
using ToDoList.API.Helpers;

namespace ToDoList.API
{
    public class DependencyRegister : IDependencyRegister
    {
        public void Register(UnityContainer container)
        {
            container.RegisterType<IUrlLocationService>(new HierarchicalLifetimeManager(), new InjectionFactory(InjectHelper));
        }

        private static IUrlLocationService InjectHelper(IUnityContainer container) =>
            new ToDoUrlLocationHelper((HttpRequestMessage) HttpContext.Current.Items["MS_HttpRequestMessage"]);

    }
}
