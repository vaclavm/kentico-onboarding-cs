using ToDoList.Contracts.Providers;

namespace ToDoList.API.Helpers
{
    internal class WebApiRoutes : IWebApiRoutes
    {
        internal const string GetToDoRoute = "GetToDo";

        public string ToDoRouteForGet => GetToDoRoute;
    }
}
