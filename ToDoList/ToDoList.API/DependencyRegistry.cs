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
            var injectionParams = new InjectionFactory(InjectHttpRequest);
            container.RegisterType<IUrlLocationService, ToDoUrlLocationHelper>(new HierarchicalLifetimeManager(), injectionParams);
        }

        private static HttpRequestMessage InjectHttpRequest(IUnityContainer container) =>
            (HttpRequestMessage) HttpContext.Current.Items["MS_HttpRequestMessage"];
    }
}
