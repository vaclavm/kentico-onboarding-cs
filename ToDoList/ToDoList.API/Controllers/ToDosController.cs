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
        private static readonly List<ToDoModel> ToDoList = new List<ToDoModel>
        {
            new ToDoModel {Id = Guid.Parse("790e8b03-aaea-46dd-9d9b-c33f3ff04090"), Text = "Dummy To Do 1"},
            new ToDoModel {Id = Guid.Parse("954eccc5-2047-4dda-bcb0-e1d8d176959d"), Text = "Dummy To Do 2"},
            new ToDoModel {Id = Guid.Parse("1d710f5d-4bbe-4654-906e-6c708e2bc410"), Text = "Dummy To Do 3"}
        };
        
        public async Task<IHttpActionResult> GetToDosAsync()
            => await Task.FromResult(Ok(ToDoList));

        [Route("{id}", Name = "GetToDo")]
        public async Task<IHttpActionResult> GetToDoAsync(Guid id)
            => await Task.FromResult(Ok(ToDoList[0]));

        [Route("")]
        public async Task<IHttpActionResult> PostToDoAsync([FromBody]ToDoModel toDoItem)
            => await Task.FromResult(CreatedAtRoute("GetToDo", new { id = ToDoList[2].Id }, ToDoList[2]));

        [Route("{id}")]
        public async Task<IHttpActionResult> PutToDoAsync(Guid id, [FromBody]ToDoModel toDoItem)
            => await Task.FromResult(StatusCode(HttpStatusCode.NoContent));

        [Route("{id}")]
        public async Task<IHttpActionResult> DeleteToDoAsync(Guid id)
            => await Task.FromResult(StatusCode(HttpStatusCode.NoContent));
    }
}
