using ToDoList.Contracts.DependencyInjection;
using ToDoList.Contracts.Repositories;

namespace ToDoList.Repository.DependencyInjection
{
    public class DependencyRegister : IDependencyRegister
    {
        public void Register(IContainer container)
            => container.RegisterType<IToDoRepository, ToDoRepository>(LifetimeManager.Hierarchical);
    }
}
