using System;

namespace ToDoList.Contracts.Providers
{
    public interface ILocator
    {
        string GetNewResourceLocation(Guid id);
    }
}
