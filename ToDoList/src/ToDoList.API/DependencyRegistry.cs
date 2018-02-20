using System.Net.Http;
using System.Web;
using Unity;
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
            container.RegisterType<HttpRequestMessage>(new InjectionFactory(InjectHttpRequest));
            container.RegisterType<IUrlLocationService, ToDoUrlLocationHelper>();
        }

        private static HttpRequestMessage InjectHttpRequest(IUnityContainer container) =>
            (HttpRequestMessage) HttpContext.Current.Items["MS_HttpRequestMessage"];

    }
}
