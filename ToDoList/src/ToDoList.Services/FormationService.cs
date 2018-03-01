using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using ToDoList.Contracts.Models;
using ToDoList.Contracts.Repositories;
using ToDoList.Contracts.Services;

[assembly: InternalsVisibleTo("ToDoList.Services.Tests")]

namespace ToDoList.Services
{
    internal class FormationService : IFormationService
    {
        private readonly IIdentifierService _identifierService;
        private readonly IToDoRepository _toDoRepository;
        private readonly IDateTimeService _dateTimeService;

        public FormationService(IToDoRepository repository, IIdentifierService identifierService, IDateTimeService dateTimeService)
        {
            _dateTimeService = dateTimeService;
            _toDoRepository = repository;
            _identifierService = identifierService;
        }

        public async Task<ToDo> CreateToDoAsync(ToDo toCreateToDo)
        {
            var now = _dateTimeService.GetCurrentDateTime();

            var newToDo = new ToDo
            {
                Id = _identifierService.GenerateIdentifier(),
                Text = toCreateToDo.Text,
                Created = now,
                LastModified = now
            };

            await _toDoRepository.AddToDoAsync(newToDo);
            return newToDo;
        }

        public async Task<ToDo> UpdateToDoAsync(ToDo toUpdateToDo)
        {
            toUpdateToDo.LastModified = _dateTimeService.GetCurrentDateTime();
            await _toDoRepository.ChangeToDoAsync(toUpdateToDo);

            return toUpdateToDo;
        }
    }
}
