using System;
using System.Runtime.CompilerServices;
using System.Web.Http.Routing;

using ToDoList.Contracts.Services;

[assembly:InternalsVisibleTo("ToDoList.API.Services.Tests")]

namespace ToDoList.API.Services
{
    internal class UrlLocationService : IUrlLocationService
    {
        private readonly UrlHelper _urlHelper;
        private readonly IRoutesService _routesService;

        public UrlLocationService(UrlHelper urlHelper, IRoutesService routesService)
        {
            _urlHelper = urlHelper;
            _routesService = routesService;
        }

        public string GetNewResourceLocation(Guid id)
        {
            return _urlHelper.Route(_routesService.ToDoRouteForGet, new {id});
        }
    }
}
