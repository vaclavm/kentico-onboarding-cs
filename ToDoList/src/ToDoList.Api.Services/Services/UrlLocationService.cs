using System;
using System.Web.Http.Routing;
using ToDoList.Contracts.Services;

namespace ToDoList.Api.Services.Services
{
    internal class UrlLocationService : IUrlLocationService
    {
        private readonly UrlHelper _urlHelper;
        private readonly IWebApiRoutes _webApiRoutes;

        public UrlLocationService(UrlHelper urlHelper, IWebApiRoutes webApiRoutes)
        {
            _urlHelper = urlHelper;
            _webApiRoutes = webApiRoutes;
        }

        public string GetNewResourceLocation(Guid id)
            => _urlHelper.Route(_webApiRoutes.ToDoRouteForGet, new {id});
    }
}
