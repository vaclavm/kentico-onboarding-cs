using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoList.Contracts.Services
{
    public interface IRetrieveService<T>
    {
        Task<IEnumerable<T>> RetrieveAllAsync();

        Task<T> RetrieveOneAsync(Guid id);

        Task<bool> IsInDatabaseAsync(Guid id);

        void ClearCache(T item);
    }
}
