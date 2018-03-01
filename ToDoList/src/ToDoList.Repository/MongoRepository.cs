using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MongoDB.Driver;

using ToDoList.Contracts.Models;
using ToDoList.Contracts.Repositories;

[assembly: InternalsVisibleTo("ToDoList.API.DependencyInjection.Tests")]

namespace ToDoList.Repository
{
    internal class MongoRepository : IToDoRepository
    {
        private readonly IMongoCollection<ToDo> _toToList;

        public MongoRepository(ConnectionConfiguration configuration)
        {
            var databaseUrl = MongoUrl.Create(configuration.ConnectionString);
            var mongoDatabase = new MongoClient(databaseUrl).GetDatabase(databaseUrl.DatabaseName);

            _toToList = mongoDatabase.GetCollection<ToDo>("ToDoItems");
        }

        public async Task<IEnumerable<ToDo>> GetToDosAsync()
            => await _toToList.Find(_ => true).ToListAsync();

        public async Task<ToDo> GetToDoAsync(Guid id)
            => await _toToList.Find(item => item.Id == id).FirstOrDefaultAsync();

        public async Task AddToDoAsync(ToDo toDoItem)
            => await _toToList.InsertOneAsync(toDoItem);

        public async Task ChangeToDoAsync(ToDo toDoItem)
            => await _toToList.ReplaceOneAsync(item => item.Id == toDoItem.Id, toDoItem);

        public async Task DeleteToDoAsync(Guid id)
            => await _toToList.FindOneAndDeleteAsync(item => item.Id == id);
    }
}