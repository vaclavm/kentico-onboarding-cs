using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Web.Http;

using ToDoList.API.Models;

namespace ToDoList.API.Controllers
{
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:apiVersion}/todos")]
    public class ToDosController : ApiController
    {
        private const string RouteId = "{id}";

        private static readonly List<ToDoModel> ToDoList = new List<ToDoModel>
        {
            new ToDoModel {Id = Guid.Parse("790e8b03-aaea-46dd-9d9b-c33f3ff04090"), Text = "Dummy To Do 1"},
            new ToDoModel {Id = Guid.Parse("954eccc5-2047-4dda-bcb0-e1d8d176959d"), Text = "Dummy To Do 2"},
            new ToDoModel {Id = Guid.Parse("1d710f5d-4bbe-4654-906e-6c708e2bc410"), Text = "Dummy To Do 3"}
        };

        [HttpGet]
        public async Task<IHttpActionResult> GetToDosAsync()
        {
            return await Task.FromResult(Ok(ToDoList));
        }

        [HttpGet]
        [Route(RouteId)]
        public async Task<IHttpActionResult> GetToDoAsync(Guid id)
        {
            return await Task.FromResult(Ok(ToDoList[0]));
        }

        [HttpPost]
        public async Task<IHttpActionResult> AddToDoAsync([FromBody]ToDoModel toDoItem)
        {
            int updadeId = 2;
            var newItemUri = $"{Request.RequestUri}/{updadeId}";
            return await Task.FromResult(Created(newItemUri, ToDoList[updadeId]));
        }

        [HttpPut]
        [Route(RouteId)]
        public async Task<IHttpActionResult> ChangeToDoAsync(Guid id, [FromBody]ToDoModel toDoItem)
        {
            return await Task.FromResult(StatusCode(HttpStatusCode.NoContent));
        }

        [HttpDelete]
        [Route(RouteId)]
        public async Task<IHttpActionResult> DeleteToDoAsync(Guid id)
        {
            return await Task.FromResult(StatusCode(HttpStatusCode.NoContent));
        }
    }
}
