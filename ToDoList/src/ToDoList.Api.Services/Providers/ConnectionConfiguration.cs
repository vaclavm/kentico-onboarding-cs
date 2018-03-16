using System.Configuration;
using ToDoList.Contracts.Providers;

namespace ToDoList.Api.Services.Providers
{
    internal class ConnectionConfiguration : IConnectionConfiguration
    {
        private string _connectionString;

        public string ConnectionString 
            => _connectionString ?? (_connectionString = ConfigurationManager.ConnectionStrings["MongoConnectionString"].ConnectionString);
    }
}
