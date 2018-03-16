using System;
using ToDoList.Contracts.Providers;

namespace ToDoList.Services.Services
{
    internal class DateTimeService : ITimeProvider
    {
        public DateTime GetCurrentDateTime()
            => DateTime.Now;
    }
}
