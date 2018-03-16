using System;

namespace ToDoList.Contracts.Providers
{
    public interface ITimeProvider
    {
        DateTime GetCurrentDateTime();
    }
}
