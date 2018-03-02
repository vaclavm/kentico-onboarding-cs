using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ToDoList.Contracts.Models;
using ToDoList.Contracts.Services;

namespace ToDoList.Api.ViewModels
{
    public class ToDoViewModel : IValidatableObject, IConvertibleObject<ToDo>
    {
        public string Value { get; set; }

        public virtual ToDo Convert()
            => new ToDo { Text = Value };

        public virtual ToDo Convert(ToDo original)
            => new ToDo { Id = original.Id, Text = Value, Created = original.Created, LastModified = original.LastModified };

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(Value))
            {
                results.Add(new ValidationResult("The ToDo value must be set", new[] { "Value" }));
            }

            return results;
        }
    }
}