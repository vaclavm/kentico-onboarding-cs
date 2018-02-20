using System;
using System.Collections;

namespace ToDoList.Contracts.Services
{
    public interface IUrlLocationService
    {
        IEnumerable Routes { get; set; }

        string GetAfterPostLocation(Guid id);
    }
}
