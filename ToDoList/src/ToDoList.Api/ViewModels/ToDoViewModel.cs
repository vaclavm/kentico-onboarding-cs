using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ToDoList.Contracts.Models;

namespace ToDoList.Api.ViewModels
{
    public class ToDoViewModel : IValidatableObject, IConvertibleObject<ToDo>
    {
        public string Text { get; set; }

        public virtual ToDo Convert()
            => new ToDo { Text = Text };

        public virtual ToDo Convert(ToDo original)
            => new ToDo { Id = original.Id, Text = Text, Created = original.Created, LastModified = original.LastModified };

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(Text))
            {
                results.Add(new ValidationResult("The ToDo value must be set", new[] { "Text" }));
            }

            return results;
        }
    }
}