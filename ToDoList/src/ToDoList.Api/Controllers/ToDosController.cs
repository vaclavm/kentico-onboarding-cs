using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Web.Http;
using ToDoList.API.Helpers;
using ToDoList.Contracts.Models;
using ToDoList.Contracts.Repositories;
using ToDoList.Contracts.Services;

namespace ToDoList.Api.Controllers
{
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:apiVersion}/todos")]
    [Route("")]
    public class ToDosController : ApiController
    {
        private readonly IToDoRepository _toDoRepository;
        private readonly IUrlLocationService _locationService;
        private readonly IFormationService _formationService;

        public ToDosController(IToDoRepository toDoRepository, IUrlLocationService locationService, IFormationService formationService)
        {
            _formationService = formationService;
            _toDoRepository = toDoRepository;
            _locationService = locationService;
        }
        
        public async Task<IHttpActionResult> GetToDosAsync()
            => Ok(await _toDoRepository.GetToDosAsync());

        [Route("{id}", Name = WebApiRoutes.GetToDoRoute)]
        public async Task<IHttpActionResult> GetToDoAsync(Guid id)
            => Ok(await _toDoRepository.GetToDoAsync(id));
        
        public async Task<IHttpActionResult> PostToDoAsync([FromBody]ToDo toDoItem)
        {
            var newToDo = await _formationService.CreateToDoAsync(toDoItem);
            var toDoLocationUrl = _locationService.GetNewResourceLocation(newToDo.Id);

            return Created(toDoLocationUrl, newToDo);
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
