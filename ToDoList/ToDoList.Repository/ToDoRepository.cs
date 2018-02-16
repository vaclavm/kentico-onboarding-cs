using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ToDoList.Contracts.Models;
using ToDoList.Contracts.Repositories;

namespace ToDoList.Repository
{
    public class ToDoRepository : IToDoRepository
    {
        private readonly List<ToDo> _toDoList = new List<ToDo>
        {
            new ToDo {Id = Guid.Parse("790e8b03-aaea-46dd-9d9b-c33f3ff04090"), Text = "Dummy To Do 1"},
            new ToDo {Id = Guid.Parse("954eccc5-2047-4dda-bcb0-e1d8d176959d"), Text = "Dummy To Do 2"},
            new ToDo {Id = Guid.Parse("1d710f5d-4bbe-4654-906e-6c708e2bc410"), Text = "Dummy To Do 3"}
        };

        public async Task<List<ToDo>> GetToDosAsync()
        {
            return await Task.FromResult(_toDoList);
        }

        public async Task<ToDo> GetToDoAsync(Guid id)
        {
            return await Task.FromResult(_toDoList[0]);
        }

        public async Task<ToDo> AddToDoAsync(ToDo toDoValue)
        {
            return await Task.FromResult(_toDoList[2]);
        }

        public async Task ChangeToDoAsync(ToDo toDoItem)
        {
            await Task.CompletedTask;
        }

        public async Task DeleteToDoAsync(Guid id)
        {
            await Task.CompletedTask;
        }
    }
}
