using System;

namespace ToDoList.Contracts.Providers
{
    public interface IIdentifierProvider
    {
        Guid GenerateIdentifier();
    }
}