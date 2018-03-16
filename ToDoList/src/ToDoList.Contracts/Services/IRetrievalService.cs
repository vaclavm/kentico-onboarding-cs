using System;
using System.Threading.Tasks;

namespace ToDoList.Contracts.Services
{
    public interface IRetrieveService<T>
    {
        Task<T> RetrieveOneAsync(Guid id);

        Task<bool> IsInDatabaseAsync(Guid id);

        void ClearCache(Guid id);
    }
}
