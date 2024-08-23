namespace LibrarySystem;

public interface IExecutableHandler<R>
{
    Task Execute(R inputReceived);
}
