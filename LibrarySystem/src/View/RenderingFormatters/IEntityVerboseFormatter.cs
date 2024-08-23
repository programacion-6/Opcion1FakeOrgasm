namespace LibrarySystem;

public interface IEntityVerboseFormatter<T> : IEntityFormatter<T> where T : EntityBase
{
    public Task LoadRelatedData();
}