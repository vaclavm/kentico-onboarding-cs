using ToDoList.Contracts.Repositories;
using ToDoList.DependencyInjection;

namespace ToDoList.Repository.DependencyInjection
{
    public class DependencyRegister : IDependencyRegister
    {
        public void Register(Container container)
            => container.RegisterType<IToDoRepository, ToDoRepository>(LifetimeManager.Hierarchical);
    }
}
