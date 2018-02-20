using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace ToDoList.API.Tests.Utilities
{
    internal static class ControllerActionExtensions
    {
        public static async Task<HttpResponseMessage> ExecuteAction<T>(this T controller, Func<T, Task<IHttpActionResult>> controllerAction) 
            where T : ApiController
        {
            var response = await controllerAction(controller);
            return await response.ExecuteAsync(CancellationToken.None);
        }
    }
}
