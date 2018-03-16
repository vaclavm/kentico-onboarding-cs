using ToDoList.Contracts.DependencyInjection;
using ToDoList.Contracts.Models;
using ToDoList.Contracts.Providers;
using ToDoList.Contracts.Services;
using ToDoList.Services.Providers;
using ToDoList.Services.Services;

namespace ToDoList.Services
{
    public class DependencyRegister : IDependencyRegister
    {
        public void Register(IContainer container)
        {
            container.RegisterType<IIdentifierProvider, IdentifierProvider>();
            container.RegisterType<ITimeProvider, TimeProvider>();
            container.RegisterTypeAsSingleton<IModificationService<ToDo>, ModificationToDoService>();
            container.RegisterTypeAsSingleton<IRetrievalService<ToDo>, RetrievalToDoService>();
        }
    }
}