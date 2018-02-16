using Unity;

namespace ToDoList.Contracts
{
    public interface IDependencyRegister
    {
        void Register(UnityContainer container);
    }
}
