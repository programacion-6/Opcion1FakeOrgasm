namespace LibrarySystem;

public interface IEntityFormatterFactory<T> where T : EntityBase {

    public IEntityFormatter<T> CreateFormatter(T entity, FormatType formatType);
}