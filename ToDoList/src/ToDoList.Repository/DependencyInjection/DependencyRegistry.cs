using ToDoList.Contracts.DependencyInjection;
using ToDoList.Contracts.Repositories;

namespace ToDoList.Repository
{
    public class DependencyRegister : IDependencyRegister
    {
        public void Register(IContainer container)
            => container.RegisterTypeAsSingleton<IToDoRepository, MongoRepository>();
    }
}
