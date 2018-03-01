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
        private readonly IRetrieveService<ToDo> _retrieveService;

        public ToDosController(IToDoRepository toDoRepository, IUrlLocationService locationService, IFormationService formationService, IRetrieveService<ToDo> retrieveService)
        {
            _retrieveService = retrieveService;
            _formationService = formationService;
            _toDoRepository = toDoRepository;
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

        public async Task<IHttpActionResult> PostToDoAsync([FromBody]ToDo toDoItem)
        {
            string toDoLocationUrl = await CreateToDoAsync(toDoItem);
            return Created(toDoLocationUrl, toDoItem);
        }
        
        [Route("{id}")]
        public async Task<IHttpActionResult> PutToDoAsync(Guid id, [FromBody]ToDo toDoItem)
        {
            if (!await _retrieveService.IsInDatabaseAsync(id))
            {
                string toDoLocationUrl = await CreateToDoAsync(toDoItem);
                return Created(toDoLocationUrl, toDoItem);
            }

            await _formationService.UpdateToDoAsync(toDoItem);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("{id}")]
        public async Task<IHttpActionResult> DeleteToDoAsync(Guid id)
        {
            if (!await _retrieveService.IsInDatabaseAsync(id))
            {
                return NotFound();
            }

            await _toDoRepository.DeleteToDoAsync(id);
            _retrieveService.ClearCache();

            return StatusCode(HttpStatusCode.NoContent);
        }

        private async Task<string> CreateToDoAsync(ToDo toDoItem)
        {
            toDoItem = await _formationService.CreateToDoAsync(toDoItem);
            string toDoLocationUrl = _locationService.GetNewResourceLocation(toDoItem.Id);

            return toDoLocationUrl;
        }
    }
}
