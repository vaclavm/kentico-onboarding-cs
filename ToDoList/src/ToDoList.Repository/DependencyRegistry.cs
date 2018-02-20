using Unity;
using Unity.Lifetime;

using ToDoList.Contracts;
using ToDoList.Contracts.Repositories;

namespace ToDoList.Repository
{
    public class DependencyRegister : IDependencyRegister
    {
        public void Register(UnityContainer container)
            => container.RegisterType<IToDoRepository, ToDoRepository>(new HierarchicalLifetimeManager());
    }
}
