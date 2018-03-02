namespace ToDoList.Contracts.Models
{
    public interface IConvertibleObject<T>
    {
        T Convert();

        T Convert(T original);
    }
}
