using ToDoList.Contracts.DependencyInjection;
using ToDoList.Contracts.Models;
using ToDoList.Contracts.Providers;
using ToDoList.Contracts.Services;
using ToDoList.Services.Services;
using ToDoList.Services.ToDoServices;

namespace ToDoList.Services
{
    public class DependencyRegister : IDependencyRegister
    {
        public void Register(IContainer container)
        {
            container.RegisterType<IIdentifierProvider, IdentifierService>();
            container.RegisterType<ITimeProvider, DateTimeService>();
            container.RegisterTypeAsSingleton<IModificationService<ToDo>, ModificationToDoService>();
            container.RegisterTypeAsSingleton<IRetrievalService<ToDo>, RetrievalToDoService>();
        }
    }
}