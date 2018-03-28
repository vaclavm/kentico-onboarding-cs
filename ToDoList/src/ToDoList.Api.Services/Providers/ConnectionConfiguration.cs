using System;
using System.Configuration;
using ToDoList.Contracts.Providers;

namespace ToDoList.Api.Services.Providers
{
    internal class ConnectionConfiguration : IConnectionConfiguration
    {
        private readonly Lazy<string> _connectionString = new Lazy<string>(() => ConfigurationManager.ConnectionStrings["MongoConnectionString"].ConnectionString);

        public string ConnectionString => _connectionString.Value;
    }
}
