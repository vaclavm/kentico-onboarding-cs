using ToDoList.Contracts.DependencyInjection;

namespace ToDoList.DependencyInjection
{
    public static class ContainerFactory
    {
        public static IContainer GetContainer() 
            => new Container.Container();
    }
}
