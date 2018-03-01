using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Web.Http;

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

            await _modificationService.UpdateAsync(toDoItem);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("{id}")]
        public async Task<IHttpActionResult> DeleteToDoAsync(Guid id)
        {
            if (!await _retrieveService.IsInDatabaseAsync(id))
            {
                return NotFound();
            }

            await _modificationService.DeleteAsync(id);
            _retrieveService.ClearCache();

            return StatusCode(HttpStatusCode.NoContent);
        }

        private async Task<string> CreateToDoAsync(ToDo toDoItem)
        {
            toDoItem = await _modificationService.CreateAsync(toDoItem);
            string toDoLocationUrl = _locationService.GetNewResourceLocation(toDoItem.Id);

            return toDoLocationUrl;
        }
    }
}
