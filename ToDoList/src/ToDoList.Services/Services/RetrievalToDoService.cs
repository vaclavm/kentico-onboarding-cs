using System;
using System.Threading.Tasks;

using ToDoList.Contracts.Models;
using ToDoList.Contracts.Repositories;
using ToDoList.Contracts.Services;

namespace ToDoList.Services.Services
{
    internal class RetrievalToDoToDoService : IRetrievalToDoService
    {
        private ToDo _cachedToDo;
        private readonly IToDoRepository _toDoRepository;

        public RetrievalToDoToDoService(IToDoRepository repository)
        {
            _toDoRepository = repository;
        }

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

        public void ClearCache(Guid id)
        {
            if (_cachedToDo.Id == id)
            {
                _cachedToDo = null;
            }
        }
    }
}
