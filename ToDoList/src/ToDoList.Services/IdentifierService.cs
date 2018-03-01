using System;

using ToDoList.Contracts.Services;

namespace ToDoList.Services
{
    public class IdentifierService : IIdentifierService
    {
        public Guid GenerateIdentifier()
            => Guid.NewGuid();
    }
}
