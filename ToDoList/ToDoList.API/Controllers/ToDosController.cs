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
    [Route("")]
    public class ToDosController : ApiController
    {
        private readonly IToDoRepository _toDoRepository;
        private readonly IUrlLocationService _locationService;

        public ToDosController(IToDoRepository toDoRepository, IUrlLocationService locationService)
        {
            _toDoRepository = toDoRepository;
            _locationService = locationService;
        }
        
        public async Task<IHttpActionResult> GetToDosAsync()
            => Ok(await _toDoRepository.GetToDosAsync());

        [Route("{id}", Name = "GetToDo")]
        public async Task<IHttpActionResult> GetToDoAsync(Guid id)
            => Ok(await _toDoRepository.GetToDoAsync(id));
        
        public async Task<IHttpActionResult> PostToDoAsync([FromBody]ToDo toDoItem)
        {
            var createdToDo = await _toDoRepository.AddToDoAsync(toDoItem);
            var toDoLocationUrl = _locationService.GetAfterPostLocation(createdToDo.Id);

            return Created(toDoLocationUrl, createdToDo);
        }
        
        [Route("{id}")]
        public async Task<IHttpActionResult> PutToDoAsync(Guid id, [FromBody]ToDo toDoItem)
        {
            await _toDoRepository.ChangeToDoAsync(toDoItem);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("{id}")]
        public async Task<IHttpActionResult> DeleteToDoAsync(Guid id)
        {
            await _toDoRepository.DeleteToDoAsync(id);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
