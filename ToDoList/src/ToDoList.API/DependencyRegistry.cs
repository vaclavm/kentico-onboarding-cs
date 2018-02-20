using System.Net.Http;
using System.Web;

using ToDoList.Contracts;
using ToDoList.Contracts.Services;
using ToDoList.API.Services;
using ToDoList.DependencyInjection;

namespace ToDoList.API
{
    public class DependencyRegister : IDependencyRegister
    {
        public void Register(Wrapper wrapper)
        {
            wrapper.RegisterType(InjectHttpRequest);
            wrapper.RegisterType<IUrlLocationService, ToDoUrlLocationService>(LifetimeManager.Hierarchical);
        }

        private static HttpRequestMessage InjectHttpRequest()
        {
            return (HttpRequestMessage) HttpContext.Current.Items["MS_HttpRequestMessage"];
        }
    }
}
