using System.Threading.Tasks;
using ToDoList.Contracts.Models;

namespace ToDoList.Contracts.Services
{
    public interface IModificationService<T>
    {
        Task<T> CreateAsync(IConvertibleObject<T> toCreate);

        Task<T> UpdateAsync(T toUpdate, IConvertibleObject<T> updateFrom);

        Task<T> DeleteAsync(T id);
    }
}
