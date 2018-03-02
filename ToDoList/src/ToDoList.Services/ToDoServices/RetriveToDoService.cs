using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoList.Contracts.Models;
using ToDoList.Contracts.Repositories;
using ToDoList.Contracts.Services;

namespace ToDoList.Services.ToDoServices
{
    internal class RetriveToDoService : IRetrieveService<ToDo>
    {
        private ToDo _cachedToDo;
        private readonly IToDoRepository _toDoRepository;

        public RetriveToDoService(IToDoRepository repository)
        {
            _toDoRepository = repository;
        }

        public async Task<IEnumerable<ToDo>> RetriveAllAsync()
            => await _toDoRepository.GetToDosAsync();

        public async Task<ToDo> RetriveOneAsync(Guid id)
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

        public void ClearCache(Guid id)
        {
            if (_cachedToDo.Id == id)
            {
                _cachedToDo = null;
            }
        }
    }
}
