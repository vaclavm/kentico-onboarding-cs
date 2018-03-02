using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Web.Http;
using ToDoList.Api.ViewModels;
using ToDoList.API.Helpers;
using ToDoList.Contracts.Models;
using ToDoList.Contracts.Services;

namespace ToDoList.Api.Controllers
{
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:apiVersion}/todos")]
    [Route("")]
    public class ToDosController : ApiController
    {
        private readonly IUrlLocationService _locationService;
        private readonly IModificationService<ToDo> _modificationService;
        private readonly IRetrieveService<ToDo> _retrieveService;

        public ToDosController(IUrlLocationService locationService, IModificationService<ToDo> modificationService, IRetrieveService<ToDo> retrieveService)
        {
            _retrieveService = retrieveService;
            _modificationService = modificationService;
            _locationService = locationService;
        }
        
        public async Task<IHttpActionResult> GetToDosAsync()
            => Ok(await _retrieveService.RetriveAllAsync());

        [Route("{id}", Name = WebApiRoutes.GetToDoRoute)]
        public async Task<IHttpActionResult> GetToDoAsync(Guid id)
        {
            if (!await _retrieveService.IsInDatabaseAsync(id))
            {
                return NotFound();
            }

            return Ok(await _retrieveService.RetriveOneAsync(id));
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

            if (!await _retrieveService.IsInDatabaseAsync(id))
            {
                (ToDo newItem, string location) = await CreateToDoAsync(toDoItem);
                return Created(location, newItem);
            }

            await _modificationService.UpdateAsync(toDoItem.Convert(await _retrieveService.RetriveOneAsync(id)));
            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("{id}")]
        public async Task<IHttpActionResult> DeleteToDoAsync(Guid id)
        {
            if (!await _retrieveService.IsInDatabaseAsync(id))
            {
                return NotFound();
            }

            var toDo = await _retrieveService.RetriveOneAsync(id);
            await _modificationService.DeleteAsync(toDo);
            _retrieveService.ClearCache(toDo);

            return StatusCode(HttpStatusCode.NoContent);
        }

        private async Task<(ToDo, string)> CreateToDoAsync(ToDoViewModel toDoItem)
        {
            var newToDoItem = await _modificationService.CreateAsync(toDoItem.Convert());
            string toDoLocationUrl = _locationService.GetNewResourceLocation(newToDoItem.Id);

            return (newToDoItem, toDoLocationUrl);
        }
    }
}
