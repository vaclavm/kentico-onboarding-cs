using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using ToDoList.Contracts.Models;

namespace ToDoList.Api.ViewModels
{
    public class ToDoViewModel : IValidatableObject, IConvertibleObject<ToDo>
    {
        public string Text { get; set; }

        public ToDo Convert()
            => new ToDo { Text = Text };

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                yield return new ValidationResult("The ToDo value must be set", new[] { nameof(Text) });
            }
        }
    }
}