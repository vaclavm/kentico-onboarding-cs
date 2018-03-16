using System.Net.Http;
using System.Web;

using ToDoList.Api.Services.Services;
using ToDoList.Contracts.Repositories;
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
            container.RegisterType<IConnectionConfiguration, ConnectionConfigurationService>(LifetimeManager.Singleton);
        }

        private static HttpRequestMessage InjectHttpRequest()
            => (HttpRequestMessage) HttpContext.Current.Items["MS_HttpRequestMessage"];
    }
}
