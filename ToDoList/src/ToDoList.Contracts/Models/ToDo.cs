using System;

namespace ToDoList.Contracts.Models
{
    public class ToDo
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastModified { get; set; }

        public override string ToString()
            => $"Id: {Id}; Text: {Text}; Created {Created}; LastModified {LastModified}";
    }
}