namespace ToDoList.Contracts.Models
{
    public interface IConvertibleObject<TObject>
    {
        TObject Convert();
    }
}
