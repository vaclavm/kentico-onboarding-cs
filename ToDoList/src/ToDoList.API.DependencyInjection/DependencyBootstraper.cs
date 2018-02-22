using System.Web.Http.Dependencies;

using ToDoList.Contracts;
using ToDoList.DependencyInjection;

namespace ToDoList.API.DependencyInjection
{
    public class DependencyBootstraper
    {
        private readonly Container _container = new Container();
        
        public void RegisterDependencies()
        {
            Register<Repository.DependencyRegister>(_container);
            Register<Services.DependencyRegister>(_container);
        }

        public void RegisterApiSingleton<TFrom, TTo>()
            where TTo : TFrom
        {
            _container.RegisterType<TFrom, TTo>(LifetimeManager.Singleton);
        }
        public IDependencyResolver CreateResolver()
        {
            return new DependencyResolver(_container.GetContainer());
        }

        private static void Register<TRegister>(Container container)
            where TRegister : IDependencyRegister, new()
        {
            var register = new TRegister();
            register.Register(container);
        }
    }
}
