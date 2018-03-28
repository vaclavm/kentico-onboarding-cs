using System.Threading.Tasks;

using ToDoList.Contracts.Models;
using ToDoList.Contracts.Providers;
using ToDoList.Contracts.Repositories;
using ToDoList.Contracts.Services;

namespace ToDoList.Services.Services
{
    internal class ModificationToDoModificationToDoService : IModificationToDoService
    {
        private readonly IIdentifierProvider _identifierProvider;
        private readonly IToDoRepository _toDoRepository;
        private readonly ITimeProvider _timeProvider;

        public ModificationToDoModificationToDoService(IToDoRepository repository, IIdentifierProvider identifierProvider, ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
            _toDoRepository = repository;
            _identifierProvider = identifierProvider;
        }

        public async Task<ToDo> CreateAsync(IConvertibleObject<ToDo> toCreate)
        {
            var now = _timeProvider.GetCurrentDateTime();

            var newToDo = toCreate.Convert();
            newToDo.Id = _identifierProvider.GenerateIdentifier();
            newToDo.Created = now;
            newToDo.LastModified = now;
            await _toDoRepository.AddToDoAsync(newToDo);

            return newToDo;
        }

        public async Task<ToDo> UpdateAsync(ToDo toUpdate, IConvertibleObject<ToDo> updateFrom)
        {
            var newToDo = updateFrom.Convert();

            toUpdate.Text = newToDo.Text;
            toUpdate.LastModified = _timeProvider.GetCurrentDateTime();
            await _toDoRepository.ChangeToDoAsync(toUpdate);

            return toUpdate;
        }
    }
}
