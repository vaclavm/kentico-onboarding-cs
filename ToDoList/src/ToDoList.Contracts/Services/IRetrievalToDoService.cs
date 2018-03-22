using System;
using System.Threading.Tasks;

using ToDoList.Contracts.Models;

namespace ToDoList.Contracts.Services
{
    public interface IRetrievalToDoService
    {
        Task<ToDo> RetrieveOneAsync(Guid id);

        Task<bool> IsInDatabaseAsync(Guid id);

        void ClearCache(Guid id);
    }
}
