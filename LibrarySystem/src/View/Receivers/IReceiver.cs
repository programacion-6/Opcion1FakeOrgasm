namespace LibrarySystem;

public interface IReceiver<I>
{
    I ReceiveInput();
}