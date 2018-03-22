using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Web.Http;
using ToDoList.Api.Providers;
using ToDoList.Api.ViewModels;
using ToDoList.Contracts.Models;
using ToDoList.Contracts.Providers;
using ToDoList.Contracts.Repositories;
using ToDoList.Contracts.Services;

namespace ToDoList.Api.Controllers
{
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:apiVersion}/todos")]
    [Route("")]
    public class ToDosController : ApiController
    {
        private readonly ILocator _locationService;
        private readonly IModificationToDoService _modificationToDoService;
        private readonly IRetrievalToDoService _retrievalToDoService;
        private readonly IToDoRepository _repositoryService;

        public ToDosController(ILocator locationService, IModificationToDoService modificationToDoService, IRetrievalToDoService retrievalToDoService, IToDoRepository repositoryService)
        {
            _retrievalToDoService = retrievalToDoService;
            _modificationToDoService = modificationToDoService;
            _locationService = locationService;
            _repositoryService = repositoryService;
        }
        
        public async Task<IHttpActionResult> GetToDosAsync()
            => Ok(await _repositoryService.GetToDosAsync());

        [Route("{id}", Name = WebApiRoutes.GetToDoRoute)]
        public async Task<IHttpActionResult> GetToDoAsync(Guid id)
        {
            if (!await _retrievalToDoService.IsInDatabaseAsync(id))
            {
                return NotFound();
            }

            var retrievedToDo = await _retrievalToDoService.RetrieveOneAsync(id);
            return Ok(retrievedToDo);
        }

        public async Task<IHttpActionResult> PostToDoAsync(ToDoViewModel toDoItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            (ToDo newItem, string location) = await CreateToDoAsync(toDoItem);
            return Created(location, newItem);
        }
        
        [Route("{id}")]
        public async Task<IHttpActionResult> PutToDoAsync(Guid id, ToDoViewModel toDoItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id == Guid.Empty)
            {
                (ToDo newItem, string location) = await CreateToDoAsync(toDoItem);
                return Created(location, newItem);
            }

            if (!await _retrievalToDoService.IsInDatabaseAsync(id))
            {
                return NotFound();
            }

            var originalToDo = await _retrievalToDoService.RetrieveOneAsync(id);
            await _modificationToDoService.UpdateAsync(originalToDo, toDoItem);

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("{id}")]
        public async Task<IHttpActionResult> DeleteToDoAsync(Guid id)
        {
            if (!await _retrievalToDoService.IsInDatabaseAsync(id))
            {
                return NotFound();
            }
            
            await _repositoryService.DeleteToDoAsync(id);
            _retrievalToDoService.ClearCache(id);

            return StatusCode(HttpStatusCode.NoContent);
        }

        private async Task<(ToDo newToDo, string location)> CreateToDoAsync(ToDoViewModel toDoItem)
        {
            var newToDoItem = await _modificationToDoService.CreateAsync(toDoItem);
            string toDoLocationUrl = _locationService.GetNewResourceLocation(newToDoItem.Id);

            return (newToDoItem, toDoLocationUrl);
        }
    }
}
