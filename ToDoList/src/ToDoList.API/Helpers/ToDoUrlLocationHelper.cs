using System;
using System.Web.Http.Routing;

using ToDoList.Contracts.Services;

namespace ToDoList.API.Helpers
{
    internal class ToDoUrlLocationHelper : IUrlLocationService
    {
        private readonly UrlHelper _urlHelper;

        public ToDoUrlLocationHelper(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public string GetAfterPostLocation(Guid id) 
            => _urlHelper.Route("GetToDo", new {id});
    }
}
