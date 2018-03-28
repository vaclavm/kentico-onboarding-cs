using System;

namespace ToDoList.Contracts.Providers
{
    public interface ILocator
    {
        string GetNewToDoLocation(Guid id);
    }
}
