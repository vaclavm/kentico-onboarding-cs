using ToDoList.DependencyInjection;

namespace ToDoList.Contracts
{
    public interface IDependencyRegister
    {
        void Register(Container container);
    }
}
