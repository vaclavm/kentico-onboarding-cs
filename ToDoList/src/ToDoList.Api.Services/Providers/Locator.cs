using System;
using System.Web.Http.Routing;
using ToDoList.Contracts.Providers;

namespace ToDoList.Api.Services.Providers
{
    internal class Locator : ILocator
    {
        private readonly UrlHelper _urlHelper;
        private readonly IWebApiRoutes _webApiRoutes;

        public Locator(UrlHelper urlHelper, IWebApiRoutes webApiRoutes)
        {
            _urlHelper = urlHelper;
            _webApiRoutes = webApiRoutes;
        }

        public string GetNewResourceLocation(Guid id)
            => _urlHelper.Route(_webApiRoutes.ToDoRouteForGet, new {id});
    }
}
