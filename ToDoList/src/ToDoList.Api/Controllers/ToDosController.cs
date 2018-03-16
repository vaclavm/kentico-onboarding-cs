﻿using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Web.Http;

using ToDoList.Api.ViewModels;
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
        private readonly IUrlLocationService _locationService;
        private readonly IModificationService<ToDo> _modificationService;
        private readonly IRetrieveService<ToDo> _retrieveService;
        private readonly IToDoRepository _repositoryService;

        public ToDosController(IUrlLocationService locationService, IModificationService<ToDo> modificationService, IRetrieveService<ToDo> retrieveService, IToDoRepository repositoryService)
        {
            _retrieveService = retrieveService;
            _modificationService = modificationService;
            _locationService = locationService;
            _repositoryService = repositoryService;
        }
        
        public async Task<IHttpActionResult> GetToDosAsync()
            => Ok(await _repositoryService.GetToDosAsync());

        [Route("{id}", Name = WebApiRoutes.GetToDoRoute)]
        public async Task<IHttpActionResult> GetToDoAsync(Guid id)
        {
            if (!await _retrieveService.IsInDatabaseAsync(id))
            {
                return NotFound();
            }

            var retrievedToDo = await _retrieveService.RetrieveOneAsync(id);
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

            if (!await _retrieveService.IsInDatabaseAsync(id))
            {
                return NotFound();
            }

            var originalToDo = await _retrieveService.RetrieveOneAsync(id);
            await _modificationService.UpdateAsync(originalToDo, toDoItem);

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("{id}")]
        public async Task<IHttpActionResult> DeleteToDoAsync(Guid id)
        {
            if (!await _retrieveService.IsInDatabaseAsync(id))
            {
                return NotFound();
            }
            
            await _repositoryService.DeleteToDoAsync(id);
            _retrieveService.ClearCache(id);

            return StatusCode(HttpStatusCode.NoContent);
        }

        private async Task<(ToDo newToDo, string location)> CreateToDoAsync(ToDoViewModel toDoItem)
        {
            var newToDoItem = await _modificationService.CreateAsync(toDoItem);
            string toDoLocationUrl = _locationService.GetNewResourceLocation(newToDoItem.Id);

            return (newToDoItem, toDoLocationUrl);
        }
    }
}
