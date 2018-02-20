using System;
using System.Net.Http;
using System.Web.Http.Routing;
using ToDoList.Contracts.Services;

namespace ToDoList.API.Helpers
{
    internal class ToDoUrlLocationHelper : IUrlLocationService
    {
        private readonly UrlHelper _urlHelper;

        public ToDoUrlLocationHelper(HttpRequestMessage requestMessage)
        {
            _urlHelper = new UrlHelper(requestMessage);
        }

        public string GetAfterPostLocation(Guid id) 
            => _urlHelper.Route("GetToDo", new {id});
    }
}
