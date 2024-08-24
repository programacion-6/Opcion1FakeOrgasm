namespace LibrarySystem;

public abstract class IEntityVerboseFormatter<T> : IEntityFormatter<T> where T : EntityBase
{
    protected IEntityVerboseFormatter(T entity) : base(entity)
    {
    }

    public abstract Task LoadRelatedData();
}