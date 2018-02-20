using ToDoList.Contracts;
using ToDoList.Contracts.Repositories;
using ToDoList.DependencyInjection;

namespace ToDoList.Repository
{
    public class DependencyRegister : IDependencyRegister
    {
        public void Register(Wrapper wrapper)
            => wrapper.RegisterType<IToDoRepository, ToDoRepository>(LifetimeManager.Hierarchical);
    }
}
