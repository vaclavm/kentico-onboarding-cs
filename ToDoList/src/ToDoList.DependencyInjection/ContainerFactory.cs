using ToDoList.Contracts.DependencyInjection;
using Unity;

namespace ToDoList.DependencyInjection
{
    public static class ContainerFactory
    {
        public static IContainer GetContainer()
        {
            return new Container.Container();
        }
    }
}
