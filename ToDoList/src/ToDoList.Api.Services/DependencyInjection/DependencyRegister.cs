using System.Net.Http;
using System.Web;
using ToDoList.Contracts.Services;
using ToDoList.DependencyInjection;
using ToDoList.DependencyInjection.Container;

namespace ToDoList.Api.Services.DependencyInjection
{
    public class DependencyRegister : IDependencyRegister
    {
        public void Register(Container container)
        {
            container.RegisterType(InjectHttpRequest);
            container.RegisterType<IUrlLocationService, UrlLocationService>(LifetimeManager.Hierarchical);
        }

        private static HttpRequestMessage InjectHttpRequest()
        {
            return (HttpRequestMessage) HttpContext.Current.Items["MS_HttpRequestMessage"];
        }
    }
}
