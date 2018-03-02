using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ToDoList.Contracts.Models;
using ToDoList.Contracts.Repositories;
using ToDoList.Contracts.Services;

[assembly: InternalsVisibleTo("ToDoList.Services.Tests")]

namespace ToDoList.Services.ToDoServices
{
    internal class RetrieveToDoService : IRetrieveService<ToDo>
    {
        private ToDo _cachedToDo;
        private readonly IToDoRepository _toDoRepository;

        public RetrieveToDoService(IToDoRepository repository)
        {
            _toDoRepository = repository;
        }

        public async Task<IEnumerable<ToDo>> RetrieveAllAsync()
            => await _toDoRepository.GetToDosAsync();

        public async Task<ToDo> RetrieveOneAsync(Guid id)
        {
            if (_cachedToDo?.Id != id)
            {
                _cachedToDo = await _toDoRepository.GetToDoAsync(id);
            }

            return _cachedToDo;
        }

        public async Task<bool> IsInDatabaseAsync(Guid id)
        {
            if (_cachedToDo?.Id == id)
            {
                return true;
            }

            _cachedToDo = await _toDoRepository.GetToDoAsync(id);

            return _cachedToDo != null;
        }

        public void ClearCache(ToDo toDo)
        {
            if (_cachedToDo.Id == toDo.Id)
            {
                _cachedToDo = null;
            }
        }
    }
}
