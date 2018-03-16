using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using ToDoList.Contracts.Models;
using ToDoList.Contracts.Repositories;
using ToDoList.Contracts.Services;

[assembly: InternalsVisibleTo("ToDoList.Services.Tests")]

namespace ToDoList.Services.ToDoServices
{
    internal class RetrievalToDoService : IRetrievalService<ToDo>
    {
        private ToDo _cachedToDo;
        private readonly IToDoRepository _toDoRepository;

        public RetrievalToDoService(IToDoRepository repository)
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
