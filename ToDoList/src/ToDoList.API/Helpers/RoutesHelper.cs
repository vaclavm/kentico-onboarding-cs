using System.Runtime.CompilerServices;

using ToDoList.Contracts.Services;

[assembly: InternalsVisibleTo("ToDoList.API.DependencyInjection.Tests")]

namespace ToDoList.API.Helpers
{
    internal class RoutesHelper : IRoutesService
    {
        internal const string GetToDoRoute = "GetToDo";

        public string ToDoRouteForGet => GetToDoRoute;
    }
}
