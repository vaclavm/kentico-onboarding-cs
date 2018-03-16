using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using ToDoList.Contracts.Models;
using ToDoList.Contracts.Repositories;
using ToDoList.Contracts.Services;

[assembly: InternalsVisibleTo("ToDoList.Services.Tests")]

namespace ToDoList.Services.ToDoServices
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

        public async Task<ToDo> CreateAsync(IConvertibleObject<ToDo> toCreate)
        {
            var now = _dateTimeService.GetCurrentDateTime();

            var newToDo = toCreate.Convert();
            newToDo.Id = _identifierService.GenerateIdentifier();
            newToDo.Created = now;
            newToDo.LastModified = now;

            await _toDoRepository.AddToDoAsync(newToDo);
            return newToDo;
        }

        public async Task<ToDo> UpdateAsync(ToDo toUpdate, IConvertibleObject<ToDo> updateFrom)
        {
            var newToDo = updateFrom.Convert();

            toUpdate.Text = newToDo.Text;
            toUpdate.LastModified = _dateTimeService.GetCurrentDateTime();
            await _toDoRepository.ChangeToDoAsync(toUpdate);

            return toUpdate;
        }
    }
}
