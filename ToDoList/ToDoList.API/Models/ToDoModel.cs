using System;

namespace ToDoList.API.Models
{
    public class ToDoModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}; Text: {Text}";
        }
    }
}