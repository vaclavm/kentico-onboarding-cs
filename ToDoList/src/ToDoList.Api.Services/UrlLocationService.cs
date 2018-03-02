using System;
using System.Runtime.CompilerServices;
using System.Web.Http.Routing;
using ToDoList.Contracts.Services;

[assembly: InternalsVisibleTo("ToDoList.API.Services.Tests")]
[assembly: InternalsVisibleTo("ToDoList.API.DependencyInjection.Tests")]

namespace ToDoList.Api.Services
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
