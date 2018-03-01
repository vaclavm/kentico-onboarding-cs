using ToDoList.Contracts.Services;
using ToDoList.DependencyInjection;
using ToDoList.DependencyInjection.Container;

namespace ToDoList.Services.DependencyInjection
{
    public class DependencyRegister : IDependencyRegister
    {
        public void Register(Container container)
        {
            container.RegisterType<IIdentifierService, IdentifierService>(LifetimeManager.Hierarchical);
            container.RegisterType<IDateTimeService, DateTimeService>(LifetimeManager.Hierarchical);
            container.RegisterType<IFormationService, FormationService>(LifetimeManager.Hierarchical);
        }
    }
}