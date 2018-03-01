using System;
using MongoDB.Bson.Serialization.Attributes;

namespace ToDoList.Contracts.Models
{
    public class ToDo
    {
        [BsonId]
        public Guid Id { get; set; }

        public string Text { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastModified { get; set; }

        public override string ToString()
            => $"Id: {Id}; Text: {Text}";
    }
}