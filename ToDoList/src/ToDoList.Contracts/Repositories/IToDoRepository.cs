using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ToDoList.Contracts.Models;

namespace ToDoList.Contracts.Repositories
{
    public interface IToDoRepository
    {
        Task<List<ToDo>> GetToDosAsync();

        Task<ToDo> GetToDoAsync(Guid id);

        Task<ToDo> AddToDoAsync(ToDo toDoValue);

        Task ChangeToDoAsync(ToDo toDoItem);

        Task DeleteToDoAsync(Guid id);
    }
}
