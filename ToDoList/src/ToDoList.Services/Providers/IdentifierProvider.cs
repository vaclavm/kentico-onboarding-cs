using System;
using ToDoList.Contracts.Providers;

namespace ToDoList.Services.Providers
{
    internal class IdentifierProvider : IIdentifierProvider
    {
        public Guid GenerateIdentifier()
            => Guid.NewGuid();
    }
}
