using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using ToDoList.Contracts.Models;
using ToDoList.Contracts.Repositories;
using ToDoList.Contracts.Services;

[assembly: InternalsVisibleTo("ToDoList.Services.Tests")]

namespace ToDoList.Services
{
    internal class ModificationToDoService : IModificationService<ToDo>
    {
        private readonly IIdentifierService _identifierService;
        private readonly IToDoRepository _toDoRepository;
        private readonly IDateTimeService _dateTimeService;

        public ModificationToDoService(IToDoRepository repository, IIdentifierService identifierService, IDateTimeService dateTimeService)
        {
            _dateTimeService = dateTimeService;
            _toDoRepository = repository;
            _identifierService = identifierService;
        }

        public async Task<ToDo> CreateAsync(ToDo toCreate)
        {
            var now = _dateTimeService.GetCurrentDateTime();

            var newToDo = new ToDo
            {
                Id = _identifierService.GenerateIdentifier(),
                Text = toCreate.Text,
                Created = now,
                LastModified = now
            };

            await _toDoRepository.AddToDoAsync(newToDo);
            return newToDo;
        }

        public async Task<ToDo> UpdateAsync(ToDo toUpdate)
        {
            toUpdate.LastModified = _dateTimeService.GetCurrentDateTime();
            await _toDoRepository.ChangeToDoAsync(toUpdate);

            return toUpdate;
        }

        public async Task DeleteAsync(Guid id)
            => await _toDoRepository.DeleteToDoAsync(id);
    }
}
