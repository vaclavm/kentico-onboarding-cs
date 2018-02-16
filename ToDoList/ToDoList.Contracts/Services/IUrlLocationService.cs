using System;

namespace ToDoList.Contracts.Services
{
    public interface IUrlLocationService
    {
        string GetAfterPostLocation(Guid id);
    }
}
