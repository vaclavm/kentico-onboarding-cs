using System.Net.Http;
using System.Web;

using ToDoList.Api.Services.Services;
using ToDoList.Contracts.Repositories;
using ToDoList.Contracts.DependencyInjection;
using ToDoList.Contracts.Services;

namespace ToDoList.Api.Services
{
    public class DependencyRegister : IDependencyRegister
    {
        public void Register(IContainer container)
        {
            container.RegisterType(InjectHttpRequest);
            container.RegisterTypeAsSingleton<IUrlLocationService, UrlLocationService>();
            container.RegisterTypeAsSingleton<IConnectionConfiguration, ConnectionConfigurationService>();
        }

        private static HttpRequestMessage InjectHttpRequest()
            => (HttpRequestMessage) HttpContext.Current.Items["MS_HttpRequestMessage"];
    }
}
