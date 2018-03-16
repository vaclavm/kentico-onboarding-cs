using System;
using ToDoList.Contracts.Services;

namespace ToDoList.Services.Services
{
    internal class IdentifierService : IIdentifierService
    {
        public Guid GenerateIdentifier()
            => Guid.NewGuid();
    }
}
