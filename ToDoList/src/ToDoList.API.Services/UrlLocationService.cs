using System;
using System.Runtime.CompilerServices;
using System.Web.Http.Routing;

using ToDoList.Contracts.Services;

[assembly: InternalsVisibleTo("ToDoList.API")]
namespace ToDoList.API.Services
{
    internal class UrlLocationService : IUrlLocationService
    {
        private readonly UrlHelper _urlHelper;

        public string ResourceGetRoute { get; set; }

        public UrlLocationService(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public string GetNewResourceLocation(Guid id)
        {
            return _urlHelper.Route(ResourceGetRoute, new {id});
        }
    }
}
