using System;

namespace ToDoList.Contracts.Services
{
    public interface IUrlLocationService
    {
        string GetNewResourceLocation(Guid id);
    }
}
