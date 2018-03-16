using System;
using ToDoList.Contracts.Services;

namespace ToDoList.Services.Services
{
    internal class DateTimeService : IDateTimeService
    {
        public DateTime GetCurrentDateTime()
            => DateTime.Now;
    }
}
