using System.Configuration;
using System.Net.Http;
using System.Web;

using ToDoList.Contracts.Repositories;
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

            var configuration = new ConnectionConfiguration { ConnectionString = ConfigurationManager.ConnectionStrings["MongoConnectionString"].ConnectionString };
            container.RegisterInstance(configuration);
        }

        private static HttpRequestMessage InjectHttpRequest()
            => (HttpRequestMessage) HttpContext.Current.Items["MS_HttpRequestMessage"];
    }
}
