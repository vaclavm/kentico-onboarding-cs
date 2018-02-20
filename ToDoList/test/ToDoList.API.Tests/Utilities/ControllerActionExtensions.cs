using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace ToDoList.API.Tests.Utilities
{
    internal static class ControllerActionExtensions
    {
        public static async Task<HttpResponseMessage> ExecuteAction<T>(this T controller, Func<T, Task<IHttpActionResult>> action) 
            where T : ApiController
        {
            var response = await action(controller);
            return await response.ExecuteAsync(CancellationToken.None);
        }
    }
}
