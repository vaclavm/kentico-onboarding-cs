using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ToDoList.Contracts.Models;

namespace ToDoList.Contracts.Repositories
{
    public interface IToDoRepository
    {
        Task<IEnumerable<ToDo>> GetToDosAsync();

        Task<ToDo> GetToDoAsync(Guid id);

        Task AddToDoAsync(ToDo toDoValue);

        Task ChangeToDoAsync(ToDo toDoItem);

        Task DeleteToDoAsync(Guid id);
    }
}
