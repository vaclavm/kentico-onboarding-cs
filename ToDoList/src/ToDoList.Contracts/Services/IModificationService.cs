using System.Threading.Tasks;

using ToDoList.Contracts.Models;

namespace ToDoList.Contracts.Services
{
    public interface IModificationToDoService
    {
        Task<ToDo> CreateAsync(IConvertibleObject<ToDo> toCreate);

        Task<ToDo> UpdateAsync(ToDo toUpdate, IConvertibleObject<ToDo> updateFrom);
    }
}
