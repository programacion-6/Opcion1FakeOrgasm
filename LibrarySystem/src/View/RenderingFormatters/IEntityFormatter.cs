namespace LibrarySystem;

public abstract class IEntityFormatter<T> where T : EntityBase
{
    protected T _entity;

    protected IEntityFormatter(T entity)
    {
        _entity = entity;
    }

    public T Entity
    {
        get => _entity;
    }
}