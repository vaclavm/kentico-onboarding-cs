using System;
using System.Threading.Tasks;

namespace ToDoList.Contracts.Services
{
    public interface IModificationService<T>
    {
        Task<T> CreateAsync(T toCreate);

        Task<T> UpdateAsync(T toUpdate);

        Task<T> DeleteAsync(T id);
    }
}
