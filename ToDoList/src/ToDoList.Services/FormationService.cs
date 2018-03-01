using System.Threading.Tasks;

using ToDoList.Contracts.Models;
using ToDoList.Contracts.Repositories;
using ToDoList.Contracts.Services;

namespace ToDoList.Services
{
    internal class FormationService : IFormationService
    {
        private readonly IIdentifierService _identifierService;
        private readonly IToDoRepository _toDoRepository;

        public FormationService(IToDoRepository repository, IIdentifierService identifierService)
        {
            _toDoRepository = repository;
            _identifierService = identifierService;
        }

        public async Task<ToDo> CreateToDoAsync(ToDo toCreateToDo)
        {
            var newToDo = new ToDo
            {
                Id = _identifierService.GenerateIdentifier(),
                Text = toCreateToDo.Text
            };

            await _toDoRepository.AddToDoAsync(newToDo);
            return newToDo;
        }
    }
}
