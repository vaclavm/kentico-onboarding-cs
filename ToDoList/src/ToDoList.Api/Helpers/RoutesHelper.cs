using ToDoList.Contracts.Services;

namespace ToDoList.API.Helpers
{
    internal class WebApiRoutes : IWebApiRoutes
    {
        internal const string GetToDoRoute = "GetToDo";

        public string ToDoRouteForGet => GetToDoRoute;
    }
}
