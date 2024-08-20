namespace LibrarySystem;

public interface IEntityRequester<T> where T : EntityBase
{
    public T? AskForEntity();
}
