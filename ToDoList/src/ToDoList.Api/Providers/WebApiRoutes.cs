using ToDoList.Contracts.Providers;

namespace ToDoList.Api.Providers
{
    internal class WebApiRoutes : IWebApiRoutes
    {
        internal const string GetToDoRoute = "GetToDo";

        public string ToDoRouteForGet => GetToDoRoute;
    }
}
