using System;

namespace ToDoList.Contracts.Models
{
    public class ToDo
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public override string ToString()
            => $"Id: {Id}; Text: {Text}";
    }
}