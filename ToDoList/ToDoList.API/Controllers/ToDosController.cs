using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Web.Http;

using ToDoList.Contracts.Models;
using ToDoList.Contracts.Repositories;

namespace ToDoList.API.Controllers
{
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:apiVersion}/todos")]
    public class ToDosController : ApiController
    {
        private readonly IToDoRepository _toDoRepository;
        private const string RouteId = "{id}";

        public ToDosController(IToDoRepository toDoRepository)
        {
            _toDoRepository = toDoRepository;
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetToDosAsync()
        {
            return Ok(await _toDoRepository.GetToDosAsync());
        }

        [HttpGet]
        [Route(RouteId)]
        public async Task<IHttpActionResult> GetToDoAsync(Guid id)
        {
            return Ok(await _toDoRepository.GetToDoAsync(id));
        }

        [HttpPost]
        public async Task<IHttpActionResult> AddToDoAsync([FromBody]ToDo toDoItem)
        {
            int updadeId = 2;
            var newItemUri = $"{Request.RequestUri}/{updadeId}";
            return Created(newItemUri, await _toDoRepository.AddToDoAsync(toDoItem));
        }

        [HttpPut]
        [Route(RouteId)]
        public async Task<IHttpActionResult> ChangeToDoAsync(Guid id, [FromBody]ToDo toDoItem)
        {
            await _toDoRepository.ChangeToDoAsync(toDoItem);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpDelete]
        [Route(RouteId)]
        public async Task<IHttpActionResult> DeleteToDoAsync(Guid id)
        {
            await _toDoRepository.DeleteToDoAsync(id);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
