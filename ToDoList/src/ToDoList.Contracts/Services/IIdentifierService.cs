using System;

namespace ToDoList.Contracts.Services
{
    public interface IIdentifierService
    {
        Guid GenerateIdentifier();
    }
}