using ToDoList.Contracts.DependencyInjection;
using ToDoList.Contracts.Models;
using ToDoList.Contracts.Services;
using ToDoList.Services.ToDoServices;

namespace ToDoList.Services.DependencyInjection
{
    public class DependencyRegister : IDependencyRegister
    {
        public void Register(IContainer container)
        {
            container.RegisterType<IIdentifierService, IdentifierService>(LifetimeManager.Hierarchical);
            container.RegisterType<IDateTimeService, DateTimeService>(LifetimeManager.Hierarchical);
            container.RegisterType<IModificationService<ToDo>, ModificationToDoService>(LifetimeManager.Singleton);
            container.RegisterType<IRetrieveService<ToDo>, RetrieveToDoService>(LifetimeManager.Singleton);
        }
    }
}