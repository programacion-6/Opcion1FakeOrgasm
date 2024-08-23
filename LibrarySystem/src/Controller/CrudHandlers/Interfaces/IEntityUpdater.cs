namespace LibrarySystem;

public interface IEntityUpdater<T, R> where T : EntityBase
{
    Task TryToUpdateEntity();
}
