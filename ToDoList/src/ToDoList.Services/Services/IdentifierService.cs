using System;

using ToDoList.Contracts.Providers;

namespace ToDoList.Services.Services
{
    internal class IdentifierService : IIdentifierProvider
    {
        public Guid GenerateIdentifier()
            => Guid.NewGuid();
    }
}
