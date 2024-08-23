namespace LibrarySystem;

public interface IEntityFormatterFactory<T> where T : EntityBase
{
    public IEntityFormatter<T>? CreateSimpleFormatter(T? entity);
    public Task<IEntityFormatter<T>?> CreateVerboseFormatter(T? entity);
}
