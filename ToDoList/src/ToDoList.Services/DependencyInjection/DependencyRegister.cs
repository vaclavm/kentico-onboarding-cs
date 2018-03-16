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
            container.RegisterType<IIdentifierService, IdentifierService>();
            container.RegisterType<IDateTimeService, DateTimeService>();
            container.RegisterTypeAsSingleton<IModificationService<ToDo>, ModificationToDoService>();
            container.RegisterTypeAsSingleton<IRetrieveService<ToDo>, RetrieveToDoService>();
        }
    }
}