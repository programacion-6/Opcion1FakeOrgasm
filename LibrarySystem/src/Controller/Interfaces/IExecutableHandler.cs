namespace LibrarySystem;

public interface IExecutableHandler<R>
{
    public void Execute(R inputReceived);
}
