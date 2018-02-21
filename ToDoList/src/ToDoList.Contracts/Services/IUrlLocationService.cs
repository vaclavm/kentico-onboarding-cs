using System;

namespace ToDoList.Contracts.Services
{
    public interface IUrlLocationService
    {
        string ResourceGetRoute { get; set; }

        string GetNewResourceLocation(Guid id);
    }
}
