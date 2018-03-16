using System.Net.Http;
using System.Web;
using ToDoList.Api.Services.Providers;
using ToDoList.Contracts.DependencyInjection;
using ToDoList.Contracts.Providers;

namespace ToDoList.Api.Services
{
    public class DependencyRegister : IDependencyRegister
    {
        public void Register(IContainer container)
        {
            container.RegisterType(InjectHttpRequest);
            container.RegisterTypeAsSingleton<ILocator, Locator>();
            container.RegisterTypeAsSingleton<IConnectionConfiguration, ConnectionConfiguration>();
        }

        private static HttpRequestMessage InjectHttpRequest()
            => (HttpRequestMessage) HttpContext.Current.Items["MS_HttpRequestMessage"];
    }
}
