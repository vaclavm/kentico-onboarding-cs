using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Web.Http.Routing;

using ToDoList.Contracts.Services;

[assembly: InternalsVisibleTo("ToDoList.API")]
namespace ToDoList.API.Services
{
    internal class ToDoUrlLocationService : IUrlLocationService
    {
        private readonly UrlHelper _urlHelper;

        public IEnumerable Routes { get; set; }

        public ToDoUrlLocationService(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public string GetAfterPostLocation(Guid id)
        {
            return _urlHelper.Route("GetToDo", new {id});
        }
    }
}
