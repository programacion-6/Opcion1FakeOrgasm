namespace LibrarySystem;

public interface IEntityCreator<T, R> where T : EntityBase
{
    Task TryToCreateEntity();
}
