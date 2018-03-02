using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoList.Contracts.Services
{
    public interface IRetrieveService<T>
    {
        Task<IEnumerable<T>> RetriveAllAsync();

        Task<T> RetriveOneAsync(Guid id);

        Task<bool> IsInDatabaseAsync(Guid id);

        void ClearCache(Guid id);
    }
}
