using System.Web.Http;
using Newtonsoft.Json.Serialization;

namespace ToDoList.Api
{
    internal static class JsonConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var formatter = config.Formatters.JsonFormatter;
            formatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            formatter.UseDataContractJsonSerializer = false;
        }
    }
}