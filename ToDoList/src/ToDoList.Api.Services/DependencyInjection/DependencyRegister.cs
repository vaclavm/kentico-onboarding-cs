using System.Net.Http;
using System.Web;

using ToDoList.Contracts.DependencyInjection;
using ToDoList.Contracts.Services;

namespace ToDoList.Api.Services.DependencyInjection
{
    public class DependencyRegister : IDependencyRegister
    {
        public void Register(IContainer container)
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
