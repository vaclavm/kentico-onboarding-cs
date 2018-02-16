using System;
using System.Net.Http;

using ToDoList.Contracts.Services;

namespace ToDoList.API.Helpers
{
    internal class ToDoUrlLocationHelper : IUrlLocationService
    {
        private readonly HttpRequestMessage _request;

        public ToDoUrlLocationHelper(HttpRequestMessage request)
        {
            _request = request;
        }

        public string GetAfterPostLocation(Guid id)
        {
            return String.Format($"{_request.RequestUri}/{id}");
        }
    }
}
