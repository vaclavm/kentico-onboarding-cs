using ToDoList.Contracts.Services;

namespace ToDoList.API.Helpers
{
    internal class RoutesHelper : IRoutesService
    {
        internal const string GetToDoRoute = "GetToDo";

        public string ToDoRouteForGet => GetToDoRoute;
    }
}
