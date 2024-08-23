namespace LibrarySystem;

public interface IEntityFormatter<T> where T : EntityBase
{
    public T Entity { get; set; }
}