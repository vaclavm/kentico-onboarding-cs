using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Web.Http;

using ToDoList.Contracts.Models;
using ToDoList.Contracts.Repositories;
using ToDoList.Contracts.Services;

namespace ToDoList.API.Controllers
{
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:apiVersion}/todos")]
    public class ToDosController : ApiController
    {
        private readonly IToDoRepository _toDoRepository;
        private readonly IUrlLocationService _locationService;
        private const string RouteId = "{id}";

        public ToDosController(IToDoRepository toDoRepository, IUrlLocationService locationService)
        {
            _toDoRepository = toDoRepository;
            _locationService = locationService;
        }

        [HttpGet]
        [Route("")]
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
        [Route("")]
        public async Task<IHttpActionResult> AddToDoAsync([FromBody]ToDo toDoItem)
        {
            var createdToDo = await _toDoRepository.AddToDoAsync(toDoItem);
            var toDoLocationUrl = _locationService.GetAfterPostLocation(createdToDo.Id);

            return Created(toDoLocationUrl, createdToDo);
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
