using System.Threading.Tasks;

using ToDoList.Contracts.Models;

namespace ToDoList.Contracts.Services
{
    public interface IFormationService
    {
        Task<ToDo> CreateToDoAsync(ToDo toCreateToDo);
    }
}
