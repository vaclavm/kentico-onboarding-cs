using System;

namespace ToDoList.Contracts.Models
{
    public class ToDo
    {
        public Guid Id { get; set; }
        public string Text { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}; Text: {Text}";
        }
    }
}