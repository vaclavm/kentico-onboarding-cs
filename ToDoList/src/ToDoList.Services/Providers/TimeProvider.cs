using System;
using ToDoList.Contracts.Providers;

namespace ToDoList.Services.Providers
{
    internal class TimeProvider : ITimeProvider
    {
        public DateTime GetCurrentDateTime()
            => DateTime.Now;
    }
}
